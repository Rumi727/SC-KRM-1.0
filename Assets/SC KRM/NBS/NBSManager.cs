using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SCKRM.NBS
{
    public static class NBSManager
    {
        public static NBSFile ReadNBSFile(string path)
        {
            using FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            binaryReader.ReadInt16();
            /*NBS Version*/ binaryReader.ReadByte();
            /*Vanilla instrument count*/ binaryReader.ReadByte();
            /*Song Length*/ short songLength = binaryReader.ReadInt16();
            /*Layer count*/ short layerCount = binaryReader.ReadInt16();
            for (int i = 0; i < 4; i++)
            {
                int length = 0;
                for (int j = 0; j < 4; j++)
                    length += binaryReader.ReadByte();

                binaryReader.ReadChars(length);
            }
            /*Song tempo*/ short tickTempo = binaryReader.ReadInt16();
            /*Auto-saving*/ binaryReader.ReadByte();
            /*Auto-saving duration*/ binaryReader.ReadByte();
            /*Time signature*/ binaryReader.ReadByte();
            /*Minutes spent*/ binaryReader.ReadInt32();
            /*Left-clicks*/ binaryReader.ReadInt32();
            /*Right-clicks*/ binaryReader.ReadInt32();
            /*Note blocks added*/ binaryReader.ReadInt32();
            /*Note blocks removed*/ binaryReader.ReadInt32();
            {
                int length = 0;
                for (int i = 0; i < 4; i++)
                    length += binaryReader.ReadByte();

                binaryReader.ReadChars(length);
            }
            /*Loop on/off*/ binaryReader.ReadByte(); //if (binaryReader.ReadByte() == 1) nbsFile.loop = true; else nbsFile.loop = false;
            /*Max loop count*/ binaryReader.ReadByte();
            /*Loop start tick*/ short loopStartTick = binaryReader.ReadInt16();

            /*Jumps to the next tick*/
            short tick;
            short tick2 = 0;

            List<NBSNote> nbsNotes = new List<NBSNote>();
            while ((tick = binaryReader.ReadInt16()) != 0)
            {
                tick2 += tick;
                List<NBSNoteMetaData> nbsNoteMetaDatas = new List<NBSNoteMetaData>();

                /*Jumps to the next layer*/
                short layerIndex;
                short layerIndex2 = 0;
                while ((layerIndex = binaryReader.ReadInt16()) != 0)
                {
                    layerIndex2 += layerIndex;
                    NBSNoteMetaData nbsNoteMetaData = new NBSNoteMetaData
                    (
                        (short)(layerIndex2 - 1),
                        /*Note block instrument*/ binaryReader.ReadByte(),
                        /*Note block key*/ binaryReader.ReadByte(),
                        /*Note block velocity*/ binaryReader.ReadByte(),
                        /*Note block panning*/ binaryReader.ReadByte(),
                        /*Note block pitch*/ binaryReader.ReadInt16()
                    );

                    nbsNoteMetaDatas.Add(nbsNoteMetaData);
                }

                nbsNotes.Add(new NBSNote(tick2, nbsNoteMetaDatas));
            }

            List<NBSLayer> nbsLayers = new List<NBSLayer>();
            for (int i = 0; i < layerCount; i++)
            {
                string layerName;
                {
                    int length = 0;
                    for (int j = 0; j < 4; j++)
                        length += binaryReader.ReadByte();

                    layerName = new string(binaryReader.ReadChars(length));
                }

                NBSLayer nbsLayer = new NBSLayer
                (
                    layerName,
                    binaryReader.ReadByte(),
                    /*Layer volume*/ binaryReader.ReadByte(),
                    /*Layer stereo*/ binaryReader.ReadByte()
                );

                nbsLayers.Add(nbsLayer);
            }

            return new NBSFile(songLength, tickTempo, loopStartTick, nbsNotes, nbsLayers);
        }
    }

    public class NBSFile
    {
        public short songLength { get; } = 0;
        public short tickTempo { get; } = 0;
        //public bool loop { get; set; } = false;
        public short loopStartTick { get; } = 0;

        public List<NBSNote> nbsNotes { get; }
        public List<NBSLayer> nbsLayers { get; }

        public NBSFile(short songLength, short tickTempo, short loopStartTick, List<NBSNote> nbsNotes, List<NBSLayer> nbsLayers)
        {
            this.songLength = songLength;
            this.tickTempo = tickTempo;
            this.loopStartTick = loopStartTick;
            this.nbsNotes = nbsNotes;
            this.nbsLayers = nbsLayers;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (nbsNotes == null)
                stringBuilder.Append("null");
            else
            {
                for (int i = 0; i < nbsNotes.Count; i++)
                    stringBuilder.Append(nbsNotes[i]);
            }

            StringBuilder stringBuilder2 = new StringBuilder();
            if (nbsLayers == null)
                stringBuilder.Append("null");
            else
            {
                for (int i = 0; i < nbsLayers.Count; i++)
                    stringBuilder.Append(nbsLayers[i]);
            }

            return $"(songLength:{songLength}, tickTempo:{tickTempo}, loopStartTick:{loopStartTick}, nbsNotes:{stringBuilder}, nbsLayers:{stringBuilder2})";
        }
    }

    public class NBSNote
    {
        public short delayTick { get; } = 0;
        public List<NBSNoteMetaData> nbsNoteMetaDatas { get; }

        public NBSNote(short delayTick, List<NBSNoteMetaData> nbsNoteMetaDatas)
        {
            this.delayTick = delayTick;
            this.nbsNoteMetaDatas = nbsNoteMetaDatas;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (nbsNoteMetaDatas == null)
                stringBuilder.Append("null");
            else
            {
                for (int i = 0; i < nbsNoteMetaDatas.Count; i++)
                    stringBuilder.Append(nbsNoteMetaDatas[i]);
            }

            return $"(delayTick:{delayTick}, nbsNoteMetaDatas:{stringBuilder})";
        }
    }

    public class NBSNoteMetaData
    {
        public short layerIndex { get; } = 0;


        public byte instrument { get; } = 0;
        public byte key { get; } = 0;
        public byte velocity { get; } = 0;
        public byte panning { get; } = 0;
        public short pitch { get; } = 0;

        public NBSNoteMetaData(short layerIndex, byte instrument, byte key, byte velocity, byte panning, short pitch)
        {
            this.layerIndex = layerIndex;
            this.instrument = instrument;
            this.key = key;
            this.velocity = velocity;
            this.panning = panning;
            this.pitch = pitch;
        }

        public override string ToString() => $"(layerIndex:{layerIndex}, instrument:{instrument}, key:{key}, velocity:{velocity}, panning:{panning}, pitch:{pitch})";
    }

    public class NBSLayer
    {
        public string layerName { get; } = "";
        public byte layerLock { get; } = 0;
        public byte layerVolume { get; } = 1;
        public byte layerStereo { get; } = 0;

        public NBSLayer(string layerName, byte layerLock, byte layerVolume, byte layerStereo)
        {
            this.layerName = layerName;
            this.layerLock = layerLock;
            this.layerVolume = layerVolume;
            this.layerStereo = layerStereo;
        }

        public override string ToString() => $"(layername:{layerName}, layerLock:{layerLock}, layerVolume:{layerStereo}, layerStereo:{layerStereo})";
    }
}