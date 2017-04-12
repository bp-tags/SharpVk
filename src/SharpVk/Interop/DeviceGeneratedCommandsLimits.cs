using System;

namespace SharpVk.Interop
{
    /// <summary>
    /// 
    /// </summary>
    public unsafe struct DeviceGeneratedCommandsLimits
    {
        /// <summary>
        /// 
        /// </summary>
        public StructureType SType; 
        
        /// <summary>
        /// 
        /// </summary>
        public void* Next; 
        
        /// <summary>
        /// 
        /// </summary>
        public uint MaxIndirectCommandsLayoutTokenCount; 
        
        /// <summary>
        /// 
        /// </summary>
        public uint MaxObjectEntryCounts; 
        
        /// <summary>
        /// 
        /// </summary>
        public uint MinSequenceCountBufferOffsetAlignment; 
        
        /// <summary>
        /// 
        /// </summary>
        public uint MinSequenceIndexBufferOffsetAlignment; 
        
        /// <summary>
        /// 
        /// </summary>
        public uint MinCommandsTokenBufferOffsetAlignment; 
    }
}
