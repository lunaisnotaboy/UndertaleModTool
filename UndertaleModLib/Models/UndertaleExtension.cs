﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace UndertaleModLib.Models
{
    public enum UndertaleExtensionKind : uint
    {
        [Obsolete("Likely unused")]
        Unknown0 = 0,
        DLL = 1,
        GML = 2,
        [Obsolete("Potentially unused before GM:S")]
        Unknown3 = 3,
        Generic = 4,
        JS = 5
    }

    public enum UndertaleExtensionVarType : uint
    {
        String = 1,
        Double = 2
    }

    public class UndertaleExtensionFunctionArg : UndertaleObject, INotifyPropertyChanged
    {
        public UndertaleExtensionVarType Type { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public UndertaleExtensionFunctionArg()
        {
            Type = UndertaleExtensionVarType.Double;
        }

        public UndertaleExtensionFunctionArg(UndertaleExtensionVarType type)
        {
            Type = type;
        }

        public void Serialize(UndertaleWriter writer)
        {
            writer.Write((uint)Type);
        }

        public void Unserialize(UndertaleReader reader)
        {
            Type = (UndertaleExtensionVarType)reader.ReadUInt32();
        }
    }

    public class UndertaleExtensionFunction : UndertaleObject, INotifyPropertyChanged
    {
        public UndertaleString Name { get; set; }
        public uint ID { get; set; } 
        public uint Kind { get; set; }
        public UndertaleExtensionVarType RetType { get; set; }
        public UndertaleString ExtName { get; set; }
        public UndertaleSimpleList<UndertaleExtensionFunctionArg> Arguments { get; set; } = new UndertaleSimpleList<UndertaleExtensionFunctionArg>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Serialize(UndertaleWriter writer)
        {
            writer.WriteUndertaleString(Name);
            writer.Write(ID);
            writer.Write(Kind);
            writer.Write((uint)RetType);
            writer.WriteUndertaleString(ExtName);
            writer.WriteUndertaleObject(Arguments);
        }

        public void Unserialize(UndertaleReader reader)
        {
            Name = reader.ReadUndertaleString();
            ID = reader.ReadUInt32();
            Kind = reader.ReadUInt32();
            RetType = (UndertaleExtensionVarType)reader.ReadUInt32();
            ExtName = reader.ReadUndertaleString();
            Arguments = reader.ReadUndertaleObject<UndertaleSimpleList<UndertaleExtensionFunctionArg>>();
        }

        public override string ToString()
        {
            return Name.Content + " (" + ExtName.Content + ")";
        }
    }

    public class UndertaleExtensionFile : UndertaleObject, INotifyPropertyChanged
    {
        public UndertaleString Filename { get; set; }
        public UndertaleString CleanupScript { get; set; }
        public UndertaleString InitScript { get; set; }
        public UndertaleExtensionKind Kind { get; set; }
        public UndertalePointerList<UndertaleExtensionFunction> Functions { get; set; } = new UndertalePointerList<UndertaleExtensionFunction>();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Serialize(UndertaleWriter writer)
        {
            writer.WriteUndertaleString(Filename);
            writer.WriteUndertaleString(CleanupScript);
            writer.WriteUndertaleString(InitScript);
            writer.Write((uint)Kind);
            writer.WriteUndertaleObject(Functions);
        }

        public void Unserialize(UndertaleReader reader)
        {
            Filename = reader.ReadUndertaleString();
            CleanupScript = reader.ReadUndertaleString();
            InitScript = reader.ReadUndertaleString();
            Kind = (UndertaleExtensionKind)reader.ReadUInt32();
            Functions = reader.ReadUndertaleObject<UndertalePointerList<UndertaleExtensionFunction>>();
        }

        public override string ToString()
        {
            return Filename.Content + " (" + GetType().Name + ")";
        }
    }

    public class UndertaleExtension : UndertaleNamedResource, INotifyPropertyChanged
    {
        public UndertaleString FolderName { get; set; }
        public UndertaleString Name { get; set; }
        public UndertaleString ClassName { get; set; }

        public UndertalePointerList<UndertaleExtensionFile> Files { get; set; } = new UndertalePointerList<UndertaleExtensionFile>();

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Name.Content + " (" + GetType().Name + ")";
        }

        public void Serialize(UndertaleWriter writer)
        {
            writer.WriteUndertaleString(FolderName);
            writer.WriteUndertaleString(Name);
            writer.WriteUndertaleString(ClassName);
            writer.WriteUndertaleObject(Files);
        }

        public void Unserialize(UndertaleReader reader)
        {
            FolderName = reader.ReadUndertaleString();
            Name = reader.ReadUndertaleString();
            ClassName = reader.ReadUndertaleString();
            Files = reader.ReadUndertaleObject<UndertalePointerList<UndertaleExtensionFile>>();
        }
    }
}
