namespace Factory
{
    /// <summary>
    /// This class represents an imaginary Game Console's storage API for the purposes of the Factory Example.
    /// </summary>
    public class GameConsole
    {
        public static StoragePermissionResponse GetStoragePermission(StoragePermissionRequest request)
            => new()
            {
                ProcessId = request.ProcessId,
                Granted = true,
                StorageHandle = Guid.NewGuid(),
                RequestType = request.RequestType
            };

#pragma warning disable CA1822 // Mark members as static
        private void Log(string operation, Guid handle, string message) => Console.WriteLine($"[{operation}|{handle}] {message}");
#pragma warning restore CA1822 // Mark members as static

        public void Save(StoragePermissionResponse? permission)
        {
            string operation = "SAVE";
            if (permission == null)
            {
                this.Log(operation, Guid.Empty, "NullArgumentException: The Permissions Object was Null.");
                return;
            }
            if (permission.Granted == false)
            {
                this.Log(operation, permission.StorageHandle, "UnauthorizedException: The Permission to Save was not Authorized.");
                return;
            }
            if (permission.RequestType != StoragePermissionRequest.StorageRequestType.ReadWrite &&
                permission.RequestType != StoragePermissionRequest.StorageRequestType.Write)
            {
                this.Log(operation, permission.StorageHandle,
                    $"InsufficientAccessException: The Permission type for this Request is insufficient ({permission.RequestType}).");
                return;
            }
            this.Log(operation, permission.StorageHandle, "Looking for Available Slot...");
            this.Log(operation, permission.StorageHandle, "Slot found.");
            this.Log(operation, permission.StorageHandle, $"Scheduling AsyncSaveTask with process id {permission.ProcessId}.");
        }

        public void Load(StoragePermissionResponse? permission)
        {
            string operation = "LOAD";
            if (permission == null)
            {
                this.Log(operation, Guid.Empty, "NullArgumentException: The Permissions Object was Null.");
                return;
            }
            if (permission.Granted == false)
            {
                this.Log(operation, permission.StorageHandle, "UnauthorizedException: The Permission to Load was not Authorized.");
                return;
            }
            if (permission.RequestType != StoragePermissionRequest.StorageRequestType.ReadWrite &&
                permission.RequestType != StoragePermissionRequest.StorageRequestType.Read)
            {
                this.Log(operation, permission.StorageHandle,
                    $"InsufficientAccessException: The Permission type for this Request is insufficient ({permission.RequestType}).");
                return;
            }
            this.Log(operation, permission.StorageHandle, $"Scheduling AsyncLoadTask with Process id {permission.ProcessId}");
        }
    }

    public class StoragePermissionRequest
    {
        public int ProcessId;
        public StorageRequestType RequestType;

        public enum StorageRequestType
        {
            Read = 0x00,
            Write = 0x01,
            ReadWrite = 0x02
        }

        public override string ToString() => $"{nameof(ProcessId)}:{ProcessId}|{nameof(RequestType)}:{RequestType}";
    }

    public class StoragePermissionResponse
    {
        public int ProcessId;
        public Guid StorageHandle;
        public bool Granted;
        public StoragePermissionRequest.StorageRequestType RequestType;

        public override string ToString()
            => $"{nameof(ProcessId)}:{ProcessId}|" +
            $"{nameof(StorageHandle)}:{StorageHandle}|" +
            $"{nameof(Granted)}:{Granted}|" +
            $"{nameof(RequestType)}:{Enum.GetName(typeof(StoragePermissionRequest.StorageRequestType), RequestType)}";
    }
}
