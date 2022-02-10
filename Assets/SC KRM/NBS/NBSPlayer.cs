using SCKRM.Object;
using SCKRM.Resource;
using SCKRM.Sound;
using SCKRM.Tool;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SCKRM.NBS
{
    [AddComponentMenu("")]
    public sealed class NBSPlayer : ObjectPooling
    {
        public NBSFile nbsFile { get; private set; }

        [SerializeField] string _nameSpace = "";
        public string nameSpace { get => _nameSpace; set => _nameSpace = value; }
        [SerializeField] string _key = "";
        public string key { get => _key; set => _key = value; }


        float tickTimer = 0;

        int _index = 0;
        public int index
        {
            get => _index;
            set
            {
                value = value.Clamp(0, nbsFile.nbsNotes.Count - 1);

                tickTimer = 0;
                _index = value;
                _tick = nbsFile.nbsNotes[value].delayTick;
            }
        }

        int _tick;
        public int tick
        {
            get => _tick;
            set
            {
                value = value.Clamp(0, length);

                tickTimer = 0;
                _tick = value;
                _index = nbsFile.nbsNotes.Select((d, i) => new { d.delayTick, index = i }).MinBy(x => (x.delayTick - value).Abs()).index;
            }
        }

        public float time
        {
            get => (_tick * 0.05f) + tickTimer;
            set
            {
                tick = (int)(value * 20);
                tickTimer = ((value * 20) - (int)(value * 20)) * 0.05f;
            }
        }
        public float realTime { get => time / tempo; set => time = value * tempo; }

        public short length => (short)(nbsFile?.songLength);
        public float realLength { get => length / tempo; }

        public bool isLooped { get; private set; } = false;
        public bool isPaused { get; set; } = false;



        #region variable
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
        #endregion



        bool allLayerLock;
        public void Refesh()
        {
            string path = "";
            for (int i = 0; i < ResourceManager.SaveData.resourcePacks.Count; i++)
            {
                string resourcePackPath = ResourceManager.SaveData.resourcePacks[i];
                if (ResourceManager.SaveData.nameSpaces.Contains(nameSpace))
                {
                    string temppath = PathTool.Combine(resourcePackPath, ResourceManager.nbsPath.Replace("%NameSpace%", nameSpace), key + ".nbs");

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
            
            nbsFile = NBSManager.ReadNBSFile(path);
            allLayerLock = nbsFile.nbsLayers.Any((b) => b.layerLock == 2);

            if (tempo < 0 && tick == 0)
            {
                tickTimer = 0;
                _tick = nbsFile.nbsNotes[nbsFile.nbsNotes.Count - 1].delayTick;
                _index = nbsFile.nbsNotes.Count - 2;
            }
            else if (tick > length)
            {
                if (loop)
                {
                    if (tempo > 0)
                        tick = nbsFile.loopStartTick;
                    else
                    {
                        tickTimer = 0;
                        _tick = nbsFile.nbsNotes[nbsFile.nbsNotes.Count - 1].delayTick;
                        _index = nbsFile.nbsNotes.Count - 2;
                    }
                }
                else
                    Remove();
            }
            else
                tick = tick;
        }



        void Update()
        {
            if (!isPaused)
            {
                if (tempo < 0)
                {
                    tickTimer -= Kernel.deltaTime * (nbsFile.tickTempo * 0.0005f) * tempo.Abs();
                    while (tickTimer <= 0)
                    {
                        _tick--;
                        tickTimer += 0.05f;

                        SoundPlay();
                    }
                }
                else
                {
                    tickTimer += Kernel.deltaTime * (nbsFile.tickTempo * 0.0005f) * tempo;
                    while (tickTimer >= 0.05f)
                    {
                        _tick++;
                        tickTimer -= 0.05f;

                        SoundPlay();
                    }
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
                        NBSLayer nbsLayer = nbsFile.nbsLayers[nbsNoteMetaData.layerIndex];
                        if (nbsLayer.layerLock != 0 && !allLayerLock)
                            continue;
                        else if (nbsLayer.layerLock != 2 && allLayerLock)
                            continue;

                        float pitch = Mathf.Pow(2, (nbsNoteMetaData.key - 45f) * 0.0833333333f) * Mathf.Pow(1.059463f, nbsNoteMetaData.pitch * 0.01f);
                        float volume = nbsNoteMetaData.velocity * 0.01f * (nbsLayer.layerVolume * 0.01f);
                        float panStereo = (nbsNoteMetaData.panning - 100) * 0.01f * ((nbsLayer.layerStereo - 100) * 0.01f);

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

                        if (spatial)
                            SoundManager.PlaySound(blockType, "minecraft", volume * this.volume, false, pitch * this.pitch, 1, panStereo + this.panStereo, minDistance, maxDistance, transform);
                        else
                            SoundManager.PlaySound(blockType, "minecraft", volume * this.volume, false, pitch * this.pitch, 1, panStereo + this.panStereo);
                    }

                    _index++;
                }
            }
            else
                _index = 0;

            if (tempo < 0)
            {
                while (index > 0 && index < nbsFile.nbsNotes.Count && nbsFile.nbsNotes[index].delayTick >= tick)
                    _index--;

                if (index == 0)
                    _index--;
            }
            else
            {
                while (index > 0 && index < nbsFile.nbsNotes.Count && nbsFile.nbsNotes[index].delayTick < tick)
                    _index++;
            }

            isLooped = false;
            if (tick < 0 || index >= nbsFile.nbsNotes.Count)
            {
                if (loop)
                {
                    if (tempo < 0)
                    {
                        tickTimer = 0;
                        _tick = nbsFile.nbsNotes[nbsFile.nbsNotes.Count - 1].delayTick;
                        _index = nbsFile.nbsNotes.Count - 2;
                    }
                    else
                    {
                        tickTimer = 0;
                        _tick = 0;
                        _index = 0;
                    }

                    isLooped = true;
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
            _tempo = 1;
            _index = 0;
            _tick = 0;

            SoundManager.nbsList.Remove(this);
            SoundObject[] soundObjects = GetComponentsInChildren<SoundObject>();
            for (int i = 0; i < soundObjects.Length; i++)
                soundObjects[i].Remove();
        }
    }
}