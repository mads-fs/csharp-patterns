#pragma warning disable CA1822 // Mark members as static

namespace Factory
{
    /// <summary>
    /// This class represents an imaginary Mobile Device's storage API for the purposes of the Factory Example.
    /// </summary>
    public class MobileDevice
    {
        private byte[] savedData = Array.Empty<byte>();
        private readonly int usrId = new Random().Next((int)(int.MaxValue * 0.5f), int.MaxValue);

        public enum MobileStorageAccess
        {
            READ,
            WRITE,
            READ_WRITE
        }

        private void Log(string operation, long handle, string message) => Console.WriteLine($"[{operation}|{handle}] {message}");

        public void Save(MobileStorageAccess access, long handle, byte[] data)
        {
            string operation = "[SYSTEM:SAVE]";
            if (access != MobileStorageAccess.WRITE && access != MobileStorageAccess.READ_WRITE)
            {
                this.Log(operation, handle, $"STORAGE_ERROR: Insufficient Access Rights ({access})");
                return;
            }
            if (data == null || data.Length == 0)
            {
                this.Log(operation, handle, $"STORAGE_ERROR: The data was invalid.");
                return;
            }
            this.Log(operation, handle, "Requesting Permissions from Kernel...");
            this.Log(operation, handle, "Authorizing Access...");
            this.Log(operation, handle, "Locking root path 'usr/share'");
            this.Log(operation, handle, $"Writing {data.Length} bytes to usr/share_{usrId}/game/data");
            this.Log(operation, handle, $"Data successfully retrieved: byte[{savedData.Length}]");
            savedData = data;
        }

        public byte[] Load(MobileStorageAccess access, long handle)
        {
            string operation = "[SYSTEM:LOAD]";
            if (access != MobileStorageAccess.READ && access != MobileStorageAccess.READ_WRITE)
            {
                this.Log(operation, handle, $"STORAGE_ERROR: Insufficient Access Rights ({access})");
                return Array.Empty<byte>();
            }
            if (savedData == null || savedData.Length == 0)
            {
                this.Log(operation, handle, "No data to retrieve");
                return Array.Empty<byte>();
            }
            this.Log(operation, handle, "Requesting Permissions from Kernel...");
            this.Log(operation, handle, "Authorizing Access...");
            this.Log(operation, handle, "Locking root path 'usr/share'");
            this.Log(operation, handle, $"Retrieving Data from usr/share_{usrId}/game/data");
            this.Log(operation, handle, $"Data successfully retrieved: byte[{savedData.Length}]");
            return savedData;
        }
    }
}