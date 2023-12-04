namespace Factory
{
    /// <summary>
    /// Here we introduce an Abstract class which implements the Interface to also provide
    /// default implementations of methods and to allow for virtual methods.
    /// </summary>
    public abstract class Storage : IStorage
    {
        public override string ToString() => "Storage";
        public abstract void Save();
        public abstract void Load();
        public virtual void Log(string message) => Console.WriteLine($"[{this}]: {message}");
    }

    public class PCStorage : Storage
    {
        private bool hasSaved = false;
        private DriveInfo? saveDrive;

        public override string ToString() => $"{nameof(PCStorage)}";

        public override void Save()
        {
            this.Log("--- SAVE ---");
            this.Log("Checking Capacity...");
            long mostSpace = 0;
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.DriveType == DriveType.Unknown ||
                    drive.DriveType == DriveType.Removable ||
                    drive.DriveType == DriveType.CDRom ||
                    drive.DriveType == DriveType.Network) continue;

                double spaceInGb = 0.00;
                if (drive.AvailableFreeSpace > 0)
                {
                    spaceInGb = drive.AvailableFreeSpace / 1024 / 1024 / 1024;
                }
                this.Log($"{drive.Name}, {drive.DriveType}, {drive.DriveFormat}, {spaceInGb}GB");
                if (drive.AvailableFreeSpace > mostSpace) saveDrive = drive;
            }
            this.Log($"Saving to {saveDrive?.Name}...");
            this.hasSaved = true;
        }

        public override void Load()
        {
            this.Log("--- LOAD ---");
            this.Log("Checking for Save...");
            if (!hasSaved) this.Log("No save was located.");
            else
            {
                this.Log($"Loading save from: {saveDrive?.Name}...");
            }
            this.Log($"Success!");
        }
    }

    public class ConsoleStorage : Storage
    {
        public override string ToString() => $"{nameof(ConsoleStorage)}";

        public override void Save()
        {
            this.Log("--- SAVE ---");
            this.Log("Requesting Storage Permissions");
            StoragePermissionRequest permissionRequest = new()
            {
                ProcessId = new Random().Next(int.MaxValue),
                RequestType = StoragePermissionRequest.StorageRequestType.ReadWrite
            };
            this.Log($"Permission Request: [{permissionRequest}]");
            StoragePermissionResponse? response = GameConsole.GetStoragePermission(permissionRequest);
            this.Log($"Permission Response: [{response}]");
            Program.ConsoleInstance?.Save(response);
        }

        public override void Load()
        {
            this.Log("--- LOAD ---");
            this.Log("Requesting Storage Permissions");
            StoragePermissionRequest permissionRequest = new()
            {
                ProcessId = new Random().Next(int.MaxValue),
                RequestType = StoragePermissionRequest.StorageRequestType.ReadWrite
            };
            this.Log($"Permission Request: [{permissionRequest}]");
            StoragePermissionResponse? response = GameConsole.GetStoragePermission(permissionRequest);
            this.Log($"Permission Response: [{response}]");
            Program.ConsoleInstance?.Load(response);
        }
    }

    public class MobileStorage : Storage
    {
        private readonly long handleId = new Random().NextInt64(long.MaxValue);
        private byte[]? data = new byte[new Random().Next((int)(int.MaxValue * 0.01f))];

        public MobileStorage() => new Random().NextBytes(data);

        public override string ToString() => $"{nameof(MobileStorage)}";
        public override void Save()
        {
            this.Log("--- SAVE ---");
            this.Log("Requesting Handle Id...");
            this.Log($"Successfully retrieved: {handleId}");
            this.Log($"Saving {data?.Length} bytes...");
            Program.MobileInstance?.Save(MobileDevice.MobileStorageAccess.WRITE, handleId, data);
        }

        public override void Load()
        {
            this.Log("--- Load ---");
            this.Log("Requesting Handle Id...");
            this.Log($"Successfully retrieved: {handleId}");
            this.Log("Loading data...");
            data = Program.MobileInstance?.Load(MobileDevice.MobileStorageAccess.READ, handleId);
            this.Log($"Successfully retrieved {data?.Length} bytes");
        }
    }
}
