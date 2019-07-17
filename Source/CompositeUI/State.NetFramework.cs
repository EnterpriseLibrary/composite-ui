using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.CompositeUI
{

    public partial class State : StateElement, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class using the provided
        /// serialization information.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="StreamingContext"/>) for this serialization. </param>
        protected State(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.id = (string)info.GetValue("id", typeof(string));
        }

        /// <summary>
        /// Populates a System.Runtime.Serialization.SerializationInfo with the data
        /// needed to serialize the target object.
        /// </summary>
        /// <param name="info">The System.Runtime.Serialization.SerializationInfo to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/>
        /// for this serialization.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("id", this.id);
        }
    }
}
