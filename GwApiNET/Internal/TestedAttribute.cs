using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET
{
    /// <summary>
    /// Used to track testing for classes, structs, and functions.
    /// <remarks>allows for auditing of a library to determine what has and hasn't been tested</remarks>
    /// </summary>
    [Tested(TestStatus.Ignored)]
    public class TestedAttribute : Attribute
    {
        /// <summary>
        /// Name of the test function or test class for the associated object or function
        /// </summary>
        public string TestReference { get; set; }
        /// <summary>
        /// Description of Test or Description of what needs to be done
        /// </summary>
        public string TestDescription { get; set; }
        /// <summary>
        /// Status of Class, Struct or Function
        /// </summary>
        public TestStatus Status { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TestedAttribute(TestStatus status = TestStatus.Tested)
            : this(string.Empty, string.Empty, status)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TestedAttribute(string reference, TestStatus status = TestStatus.Tested) : this(reference, string.Empty, status)
        {
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TestedAttribute(string reference, string description, TestStatus status = TestStatus.Tested)
        {
            TestReference = reference;
            TestDescription = description;
            Status = status;
            if (status == TestStatus.Undocumented)
                throw new ArgumentException("TestStatus.Undocumented should not be used.  This is for Audit purposes only.");
        }

        /// <summary>
        /// Test Status
        /// </summary>
        public enum TestStatus
        {
            /// <summary>
            /// Unit Test has been written
            /// </summary>
            Tested,
            /// <summary>
            /// Unit Test has not been written/completed
            /// </summary>
            Untested,
            /// <summary>
            /// Unit Testing is ignored (Not needed?)
            /// </summary>
            Ignored,
            /// <summary>
            /// Used for Audit purposes,
            /// </summary>
            Undocumented,
        }
    }
}
