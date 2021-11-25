using SCKRM.Object;
using SCKRM.Resource;
using SCKRM.Sound;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SCKRM.NBS
{
    public class NBSPlayer : ObjectPooling
    {
        public NBSFile nbsFile { get; } = new NBSFile();

        [SerializeField] string _nameSpace = "";
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }
        [SerializeField] string _key = "";
        public string key { get => _key; set => _key = value; }


        [SerializeField] float _volume = 1;
        [SerializeField] bool _loop = false;
        [SerializeField] float _tempo = 1;
        [SerializeField] float _pitch = 1;
        [SerializeField] bool _spatial = false;
        [SerializeField] float _panStereo = 0;
        [SerializeField] float _minDistance = 0;
        [SerializeField] float _maxDistance = 48;
        [SerializeField] Vector3 _localPosition = Vector3.zero;
        public float volume { get => _volume; set => _volume = value; }
        public bool loop { get => _loop; set => _loop = value; }
        public float pitch { get => _pitch; set => _pitch = value; }
        public float tempo { get => _tempo; set => _tempo = value; }
        public bool spatial { get => _spatial; set => _spatial = value; }
        public float panStereo { get => _panStereo; set => _panStereo = value; }
        public float minDistance { get => _minDistance; set => _minDistance = value; }
        public float maxDistance { get => _maxDistance; set => _maxDistance = value; }
        public Vector3 localPosition { get => _localPosition; set => _localPosition = value; }



        int _tick;
        public int tick
        {
            get => _tick;
            set
            {
                index = nbsFile.nbsNotes.Select((d, i) => new { d.delayTick, index = i }).MinBy(x => (x.delayTick - value).Abs()).index;
                _tick = value;
                timer = 0;
            }
        }



        static string tempNameSpace = "";
        static string tempKey = "";
        public void Refesh()
        {
            string path = "";
            for (int i = 0; i < ResourceManager.resourcePacks.Count; i++)
            {
                string resourcePackPath = ResourceManager.resourcePacks[i];
                if (ResourceManager.nameSpaces.Contains(nameSpace))
                {
                    string temppath = KernelMethod.PathCombine(resourcePackPath, ResourceManager.nbsPath.Replace("%NameSpace%", nameSpace), key + ".nbs");

                    if (File.Exists(temppath))
                    {
                        path = temppath;
                        break;
                    }
                }
            }

            if (!File.Exists(path))
            {
                Remove();
                return;
            }

            if (!SoundManager.nbsList.Contains(this))
                SoundManager.nbsList.Add(this);

            using FileStream fileStream = File.OpenRead(path);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            binaryReader.ReadInt16();
            /*NBS Version*/
            binaryReader.ReadByte();
            /*Vanilla instrument count*/
            binaryReader.ReadByte();
            /*Song Length*/
            nbsFile.songLength = binaryReader.ReadInt16();
            /*Layer count*/
            binaryReader.ReadInt16();
            for (int i = 0; i < 4; i++)
            {
                int length = 0;
                for (int j = 0; j < 4; j++)
                    length += binaryReader.ReadByte();

                for (int j = 0; j < length; j++)
                    binaryReader.BaseStream.Position++;
            }
            /*Song tempo*/
            nbsFile.tickTempo = binaryReader.ReadInt16();
            /*Auto-saving*/
            binaryReader.ReadByte();
            /*Auto-saving duration*/
            binaryReader.ReadByte();
            /*Time signature*/
            binaryReader.ReadByte();
            /*Minutes spent*/
            binaryReader.ReadInt32();
            /*Left-clicks*/
            binaryReader.ReadInt32();
            /*Right-clicks*/
            binaryReader.ReadInt32();
            /*Note blocks added*/
            binaryReader.ReadInt32();
            /*Note blocks removed*/
            binaryReader.ReadInt32();
            {
                int length = 0;
                for (int i = 0; i < 4; i++)
                    length += binaryReader.ReadByte();

                for (int i = 0; i < length; i++)
                    binaryReader.BaseStream.Position++;
            }
            /*Loop on/off*/
            binaryReader.ReadByte(); //if (binaryReader.ReadByte() == 1) nbsFile.loop = true; else nbsFile.loop = false;
            /*Max loop count*/
            binaryReader.ReadByte();
            /*Loop start tick*/
            nbsFile.loopStartTick = binaryReader.ReadInt16();

            /*Jumps to the next tick*/
            short tick;
            short tick2 = 0;

            nbsFile.nbsNotes.Clear();
            while ((tick = binaryReader.ReadInt16()) != 0)
            {
                tick2 += tick;
                NBSNote nbsNote = new NBSNote() { delayTick = tick2 };

                /*Jumps to the next layer*/
                while (binaryReader.ReadInt16() != 0)
                {
                    NBSNoteMetaData nbsNoteMetaData = new NBSNoteMetaData
                    {
                        /*Note block instrument*/
                        instrument = binaryReader.ReadByte(),
                        /*Note block key*/
                        key = binaryReader.ReadByte(),
                        /*Note block velocity*/
                        velocity = binaryReader.ReadByte(),
                        /*Note block panning*/
                        panning = binaryReader.ReadByte(),
                        /*Note block pitch*/
                        pitch = binaryReader.ReadInt16()
                    };

                    nbsNote.nbsNoteMetaDatas.Add(nbsNoteMetaData);
                }

                nbsFile.nbsNotes.Add(nbsNote);
            }

            if (tempNameSpace != nameSpace || tempKey != key)
            {
                _tick = 0;
                index = 0;
                timer = 0;

                tempNameSpace = nameSpace;
                tempKey = key;
            }
        }

        float timer = 0;
        int index = 0;
        void Update()
        {
            if (tempo < 0)
            {
                timer += Time.deltaTime * (nbsFile.tickTempo / 2000) * tempo.Abs();
                while (timer >= 0.05f)
                {
                    _tick--;
                    timer -= 0.05f;

                    SoundPlay();
                }
            }
            else
            {
                timer += Time.deltaTime * (nbsFile.tickTempo / 2000) * tempo;
                while (timer >= 0.05f)
                {
                    _tick++;
                    timer -= 0.05f;

                    SoundPlay();
                }
            }

            transform.localPosition = localPosition;
        }

        void SoundPlay()
        {
            if (index >= 0)
            {
                NBSNote nbsNote = nbsFile.nbsNotes[index];
                if ((tempo < 0 && nbsNote.delayTick >= tick) || (tempo >= 0 && nbsNote.delayTick <= tick))
                {
                    for (int i = 0; i < nbsNote.nbsNoteMetaDatas.Count; i++)
                    {
                        NBSNoteMetaData nbsNoteMetaData = nbsNote.nbsNoteMetaDatas[i];
                        float pitch = Mathf.Pow(2, (nbsNoteMetaData.key - 45f) / 12f) * Mathf.Pow(1.059463f, nbsNoteMetaData.pitch * 0.01f);
                        float volume = nbsNoteMetaData.velocity * 0.01f * 0.5f;
                        float panStereo = (nbsNoteMetaData.panning - 100) * 0.01f;

                        string blockType = "block.note_block.";
                        if (nbsNoteMetaData.instrument == 0)
                            blockType += "harp";
                        else if (nbsNoteMetaData.instrument == 1)
                            blockType += "bass";
                        else if (nbsNoteMetaData.instrument == 2)
                            blockType += "bassdrum";
                        else if (nbsNoteMetaData.instrument == 3)
                            blockType += "snare";
                        else if (nbsNoteMetaData.instrument == 4)
                            blockType += "hat";
                        else if (nbsNoteMetaData.instrument == 5)
                            blockType += "guitar";
                        else if (nbsNoteMetaData.instrument == 6)
                            blockType += "flute";
                        else if (nbsNoteMetaData.instrument == 7)
                            blockType += "bell";
                        else if (nbsNoteMetaData.instrument == 8)
                            blockType += "chime";
                        else if (nbsNoteMetaData.instrument == 9)
                            blockType += "xylophone";
                        else if (nbsNoteMetaData.instrument == 10)
                            blockType += "iron_xylophone";
                        else if (nbsNoteMetaData.instrument == 11)
                            blockType += "cow_bell";
                        else if (nbsNoteMetaData.instrument == 12)
                            blockType += "didgeridoo";
                        else if (nbsNoteMetaData.instrument == 13)
                            blockType += "bit";
                        else if (nbsNoteMetaData.instrument == 14)
                            blockType += "banjo";
                        else if (nbsNoteMetaData.instrument == 15)
                            blockType += "pling";

                        SoundManager.PlaySound(blockType, "minecraft", volume * this.volume, false, pitch * this.pitch, 1, panStereo + this.panStereo, spatial, minDistance, maxDistance, transform);
                    }

                    index++;
                }
            }

            if (tempo < 0)
            {
                while (index > 0 && index < nbsFile.nbsNotes.Count && nbsFile.nbsNotes[index].delayTick >= tick)
                    index--;

                if (index == 0)
                    index--;
            }
            else
            {
                while (index > 0 && index < nbsFile.nbsNotes.Count && nbsFile.nbsNotes[index].delayTick < tick)
                    index++;
            }

            if (tick < 0 || index >= nbsFile.nbsNotes.Count)
            {
                if (loop)
                {
                    if (tempo < 0)
                    {
                        timer = 0;
                        tick = nbsFile.nbsNotes[nbsFile.nbsNotes.Count - 1].delayTick;
                        index = nbsFile.nbsNotes.Count - 2;
                    }
                    else
                    {
                        timer = 0;
                        tick = 0;
                        index = 0;
                    }
                }
                else
                    Remove();
            }
        }

        public override void Remove()
        {
            base.Remove();

            nameSpace = "";
            key = "";
            tempo = 1;
            index = 0;
            _tick = 0;

            SoundManager.nbsList.Remove(this);
            SoundObject[] soundObjects = GetComponentsInChildren<SoundObject>();
            for (int i = 0; i < soundObjects.Length; i++)
                soundObjects[i].Remove();
        }

        public class NBSFile
        {
            public short songLength { get; set; } = 0;
            public float tickTempo { get; set; } = 0;
            //public bool loop { get; set; } = false;
            public short loopStartTick { get; set; } = 0;

            public List<NBSNote> nbsNotes { get; } = new List<NBSNote>();
        }

        public class NBSNote
        {
            public short delayTick { get; set; } = 0;
            public List<NBSNoteMetaData> nbsNoteMetaDatas { get; } = new List<NBSNoteMetaData>();
        }

        public class NBSNoteMetaData
        {
            public byte instrument { get; set; } = 0;
            public byte key { get; set; } = 0;
            public byte velocity { get; set; } = 0;
            public byte panning { get; set; } = 0;
            public short pitch { get; set; } = 0;
        }
    }
}