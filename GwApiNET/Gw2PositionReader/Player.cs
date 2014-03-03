using GwApiNET.CacheStrategy;
using GwApiNET.ResponseObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GwApiNET.Gw2PositionReader
{
    /// <summary>
    /// Object containing player data and methods for retrieving that data.
    /// </summary>
    public class Player : IDisposable
    {
        //public const float InchesPerMeter = 39.37010F;

        #region Structs

        /// <summary>
        /// Basic mumble link positional audio structure.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public unsafe struct LinkedMem
        {
            public uint uiVersion;//4
            public uint uiTick;//4
            public fixed float fAvatarPosition[3];//12
            public fixed float fAvatarFront[3];//12
            public fixed float fAvatarTop[3];//12
            public fixed byte name[512];//512
            public fixed float fCameraPosition[3];//12
            public fixed float fCameraFront[3];//12
            public fixed float fCameraTop[3];//12
            public fixed byte identity[512];//512
            public uint context_len;//4
            //public fixed byte context[512];//512
            public fixed byte serverAddress[28]; // context[0], contains sockaddr_in or sockaddr_in6
            public uint mapId; // context[28]
            public uint mapType; // context[32]
            public uint worldId; // context[36]
            public uint instance; // context[40]
            public uint build; // context[44]
            public fixed byte contextUnused[464];
            public fixed byte description[4096];//4096
        }

        /// <summary>
        /// GW2 uses this to store player identity info. According to 
        /// mumble.sourceforge.net/link, the identity should contain data that
        /// uniquely identifies the player in the given context.
        /// </summary>
        internal class Identity : ResponseObject
        {
            [JsonProperty("name")]
            public string Name;

            [JsonProperty("profession")]
            public uint Profession;

            [JsonProperty("map_id")]
            public uint MapId;

            [JsonProperty("world_id")]
            public uint WorldId;

            [JsonProperty("team_color_id")]
            public uint TeamColorId;

            [JsonProperty("commander")]
            public bool IsCommander;
        }

        /*
        /// <summary>
        /// Data containing information about the player from GW2's
        /// implementation of Mumble's positional-audio Link plugin.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PlayerData
        {
            public uint version;

            public uint tick;

            public Vector3 avatarPosition;

            public Vector3 avatarFront;

            public Vector3 avatarTop;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string name;

            public Vector3 cameraPosition;

            public Vector3 cameraFront;

            public Vector3 cameraTop;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string identityString;

            public uint contextLength;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 28)]
            public byte[] serverAddress; // context[0], contains sockaddr_in or sockaddr_in6

            public uint mapId; // context[28]

            public uint mapType; // context[32]

            public uint worldId; // context[36]

            public uint instance; // context[40]

            public uint build; // context[44]

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 464)]
            public byte[] contextUnused;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2048)]
            public string description;
        }
        */

        #endregion Structs

        #region Properties

        /// <summary>
        /// Link Version
        /// </summary>
        public uint Version
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->uiVersion;
                    }
                }
            }
        }

        /// <summary>
        /// Tick Value
        /// </summary>
        public uint Tick
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->uiTick;
                    }
                }
            }
        }

        /// <summary>
        /// Position of the avatar in meters. Uses left handed coordinate system.
        /// </summary>
        public Vector3 AvatarPosition
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fAvatarPosition[0],
                            Y = _data->fAvatarPosition[1],
                            Z = _data->fAvatarPosition[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Unit vector pointing out of the avatar's eyes in meters. Uses left handed 
        /// coordinate system.
        /// </summary>
        public Vector3 AvatarFront
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fAvatarFront[0],
                            Y = _data->fAvatarFront[1],
                            Z = _data->fAvatarFront[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Unit vector pointing out of the top of the avatar's head in meters. Uses 
        /// left handed coordinate system.
        /// </summary>
        public Vector3 AvatarTop
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fAvatarTop[0],
                            Y = _data->fAvatarTop[1],
                            Z = _data->fAvatarTop[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Link Name
        /// </summary>
        public string LinkName
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        byte[] strBytes = new byte[512];
                        IntPtr strBytesIntPtr = new IntPtr((void*)_data->name);
                        Marshal.Copy(strBytesIntPtr, strBytes, 0, 512);
                        return Encoding.Unicode.GetString(strBytes, 0, 512).Trim('\0');
                    }
                }
            }
        }

        /// <summary>
        /// Position of the camera in meters. Uses left handed coordinate system.
        /// </summary>
        public Vector3 CameraPosition
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fCameraPosition[0],
                            Y = _data->fCameraPosition[1],
                            Z = _data->fCameraPosition[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Unit vector pointing out of the front of the camera in meters. Uses
        /// left handed coordinate system.
        /// </summary>
        public Vector3 CameraFront
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fCameraFront[0],
                            Y = _data->fCameraFront[1],
                            Z = _data->fCameraFront[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Unit vector pointing out of the top of the camera in meters. Uses 
        /// left handed coordinate system.
        /// </summary>
        public Vector3 CameraTop
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return new Vector3
                        {
                            X = _data->fCameraTop[0],
                            Y = _data->fCameraTop[1],
                            Z = _data->fCameraTop[2]
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Returns character name of player.
        /// </summary>
        public string CharacterName
        {
            get
            {
                if (_id == null || _id.Expired)
                {
                    RefreshIdentity();
                }
                return _id.Name;
            }
        }

        /// <summary>
        /// Returns profession. Ex: 1 = Guardian.  Will change to Enumeration.
        /// </summary>
        public Profession Profession
        {
            get
            {
                if (_id == null || _id.Expired)
                {
                    RefreshIdentity();
                }
                return (Profession)_id.Profession;
            }
        }

        /// <summary>
        /// Team Color ID.
        /// </summary>
        public TeamColor TeamColor
        {
            get
            {
                if (_id == null || _id.Expired)
                {
                    RefreshIdentity();
                }
                return (TeamColor)_id.TeamColorId;
            }
        }

        /// <summary>
        /// Returns true if commander is enabled for player character.
        /// </summary>
        public bool IsCommander
        {
            get
            {
                if (_id == null || _id.Expired)
                {
                    RefreshIdentity();
                }
                return _id.IsCommander;
            }
        }

        

        /// <summary>
        /// Returns the IPEndPoint of the server. Includes IP address and port.
        /// </summary>
        public IPEndPoint ServerAddress
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        byte[] serverAddressBytes = new byte[28];
                        IntPtr serverAddressBytesIntPtr = new IntPtr((void*)_data->serverAddress);
                        Marshal.Copy(serverAddressBytesIntPtr, serverAddressBytes, 0, 28);
                        return GetServerAddress(serverAddressBytes);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Map ID of the current map.
        /// </summary>
        public uint MapId
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->mapId;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the Map Type of the current map.
        /// </summary>
        public uint MapType
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->mapType;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the current World/Shard ID, ex. 1006 = Sorrow's Furnace.
        /// </summary>
        public uint WorldId
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->worldId;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the current map instance.
        /// </summary>
        public uint Instance
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->instance;
                    }
                }
            }
        }

        /// <summary>
        /// Returns the current client build number.
        /// </summary>
        public uint Build
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        return _data->build;
                    }
                }
            }
        }

        /// <summary>
        /// Link Description
        /// </summary>
        public string Description
        {
            get
            {
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);

                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        byte[] strBytes = new byte[512];
                        IntPtr strBytesIntPtr = new IntPtr((void*)_data->description);
                        Marshal.Copy(strBytesIntPtr, strBytes, 0, 512);
                        return Encoding.Unicode.GetString(strBytes, 0, 512).Trim('\0');
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the max age, in seconds, of the Mumble Link Identity field cache strategy.  Properties
        /// from the Identity field are: <see cref="CharacterName"/>, <see cref="Profession"/>, 
        /// <see cref="TeamColor"/>, and <see cref="IsCommander"/>.
        /// </summary>
        public int IdentityCacheMaxAge
        {
            get
            {
                if (_id == null)
                {
                    RefreshIdentity();
                }
                AgeCacheStrategy strategy = _id.CacheStrategy as AgeCacheStrategy;
                int maxAge = (strategy == null ? 0 : strategy.MaxAge.Seconds);
                return maxAge;
            }
            set
            {
                int maxAge = value;
                _id.CacheStrategy = new AgeCacheStrategy(TimeSpan.FromSeconds(maxAge));
            }
        }

        #endregion Properties

        MemoryMappedFile _mappedFile;
        MemoryMappedViewAccessor _accessor;
        LinkedMem data = new LinkedMem();
        Identity _id = null;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Player()
        {

        }

        /// <summary>
        /// Start updating PlayerData from memory-mapped file.
        /// </summary>
        private void Start()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop updating PlayerData from memory-mapped file.
        /// </summary>
        private void Stop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the memory-mapped file.
        /// </summary>
        private void OpenMumbleLink()
        {
            _mappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", Marshal.SizeOf(data));
            _accessor = _mappedFile.CreateViewAccessor(0, Marshal.SizeOf(data));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenBinary(string filePath)
        {
            // Will throw exception if file at filePath is open by another program.
            _mappedFile = MemoryMappedFile.CreateFromFile(filePath, System.IO.FileMode.Open);
            _accessor = _mappedFile.CreateViewAccessor(0, Marshal.SizeOf(data), MemoryMappedFileAccess.Read);
        }

        /// <summary>
        /// Reads Identity and converts JSON string.
        /// </summary>
        private void RefreshIdentity()
        {
            // Make sure the memory-mapped file is loaded
            if (_mappedFile == null) OpenMumbleLink();

            // Read memory-mapped file
            _accessor.Read(0, out data);

            unsafe
            {
                fixed (LinkedMem* _data = &data)
                {
                    string str;
                    byte[] strBytes = new byte[512];
                    IntPtr strBytesIntPtr = new IntPtr((void*)_data->identity);
                    Marshal.Copy(strBytesIntPtr, strBytes, 0, 512);
                    str = Encoding.Unicode.GetString(strBytes);
                    str = str.Substring(0, str.IndexOf('}') + 1);
                    _id = new Identity();
                    if (str.Length > 0)
                    {
                        _id = JsonConvert.DeserializeObject<Identity>(str);
                    }
                }
            }
        }

        /*
        /// <summary>
        /// Manually updates PlayerData from memory-mapped file.
        /// </summary>
        public void Update()
        {
            Stopwatch sw = new Stopwatch();
            decimal avg = 0;
            for (int i = 0; i < 10000; i++)
            {
                sw.Start();
                // Make sure the memory-mapped file is loaded
                if (_mappedFile == null) OpenMumbleLink();

                // Read memory-mapped file
                _accessor.Read(0, out data);
                
                unsafe
                {
                    fixed (LinkedMem* _data = &data)
                    {
                        
                        byte[] tempBytes = new byte[512];
                        IntPtr tempByteIntPtr = new IntPtr((void*)_data->context);
                        Marshal.Copy(tempByteIntPtr, tempBytes, 0, 512);
                        

                        
                        float f = _data->fAvatarPosition[0];
                        

                        int size = Marshal.SizeOf(typeof(LinkedMem));
                        byte[] dataBytes = new byte[size];
                        IntPtr dataBytesIntPtr = new IntPtr((void*)_data);
                        Marshal.Copy(dataBytesIntPtr, dataBytes, 0, size);
                        PlayerData pd = dataBytes.ToStruct<PlayerData>();
                    }
                }
                sw.Stop();
                avg += sw.ElapsedTicks;
                sw.Reset();
            }
            avg /= 10000;
            Populate();
        }

        /// <summary>
        /// Manually updates PlayerData from byte array.
        /// </summary>
        /// <param name="byteArray">Data from which to populate the Player object.</param>
        public void Update(byte[] byteArray)
        {
            if (Marshal.SizeOf(typeof(PlayerData)) != byteArray.Length)
            {
                throw new ArgumentException(String.Format(
                    "The parameter {0} (length {1}) is invalid for converting to PlayerData struct.",
                    (MethodBase.GetCurrentMethod().GetParameters()[0]).Name,
                    byteArray.Length));
            }
            _pd = byteArray.ToStruct<PlayerData>();
            Populate();
        }

        /// <summary>
        /// Poll the memory-mapped file for data.
        /// </summary>
        private void Populate()
        {
            PopulateIdentity();
            PopulateServerAddress();
        }

        /// <summary>
        /// Parses identity string into Identity struct using JsonConvert.
        /// </summary>
        private void PopulateIdentity()
        {
            string identityJson = _pd.identityString.Substring(0, _pd.identityString.IndexOf('}') + 1);
            if (identityJson.Length > 0)
            {
                _pi = JsonConvert.DeserializeObject<Identity>(identityJson);
            }
        }

        /// <summary>
        /// Parses server address byte array into IPEndPoint.
        /// </summary>
        private void PopulateServerAddress()
        {
            _serverAddress = GetServerAddress(_pd.serverAddress);
        }
*/

        /// <summary>
        /// Helper method to convert byte array into IPEndpoint.
        /// </summary>
        /// <param name="serverAddressBytes"></param>
        /// <returns></returns>
        public static IPEndPoint GetServerAddress(byte[] serverAddressBytes)
        {
            if (serverAddressBytes.Length < 2)
            {
                return new IPEndPoint(IPAddress.Any, 0);
            }
            AddressFamily addressFamily = (AddressFamily)BitConverter.ToInt16(serverAddressBytes, 0);
            int sockAddrSructureSize;
            IPEndPoint ipEndPointAny;
            IPEndPoint result;
            switch (addressFamily)
            {
                case AddressFamily.InterNetwork:
                    // IP v4 address
                    sockAddrSructureSize = 16;
                    ipEndPointAny = new IPEndPoint(IPAddress.Any, 0);
                    break;
                case AddressFamily.InterNetworkV6:
                    // IP v6 address
                    sockAddrSructureSize = 28;
                    ipEndPointAny = new IPEndPoint(IPAddress.IPv6Any, 0);
                    break;
                default:
                    return new IPEndPoint(IPAddress.Any, 0);
                //throw new ArgumentOutOfRangeException("pSockaddrStructure", "Unknown address family");
            }

            var socketAddress = new SocketAddress(AddressFamily.Unspecified, sockAddrSructureSize);
            for (int i = 0; i < sockAddrSructureSize; i++)
            {
                socketAddress[i] = serverAddressBytes[i];
            }
            result = (IPEndPoint)ipEndPointAny.Create(socketAddress);
            return result;
        }

        public void Dispose()
        {
            
            _mappedFile.Dispose();
        }
    }
}
