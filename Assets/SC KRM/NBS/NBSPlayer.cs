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
    public sealed class NBSPlayer : SoundPlayerManager
    {
        public SoundData<NBSMetaData> soundData { get; private set; }
        public NBSMetaData nbsMetaData { get; private set; }
        public NBSFile nbsFile => nbsMetaData.nbsFile;


        float tickTimer = 0;

        int _index = 0;
        public int index
        {
            get => _index;
            set
            {
                value.ClampRef(0, nbsFile.nbsNotes.Count - 1);

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
                value.ClampRef(0, (int)length);

                tickTimer = 0;
                _tick = value;
                _index = nbsFile.nbsNotes.Select((d, i) => new { d.delayTick, index = i }).MinBy(x => (x.delayTick - value).Abs()).index;
            }
        }

        public override float time
        {
            get => (_tick * 0.05f) + tickTimer;
            set
            {
                tick = (int)(value * 20);
                tickTimer = ((value * 20) - (int)(value * 20)) * 0.05f;
            }
        }
        public override float realTime { get => time / tempo; set => time = value * tempo; }

        public override float length => (float)(nbsMetaData?.nbsFile.songLength);
        public override float realLength { get => length / tempo; }

        public override bool isLooped { get; protected set; } = false;
        public override bool isPaused { get; set; } = false;

        #region variable
        [SerializeField] NBSFile _selectedNBSFile = null;
        public NBSFile selectedNBSFile { get => _selectedNBSFile; set => _selectedNBSFile = value; }
        public NBSFile loadedNBSFile { get; private set; }
        #endregion



        void Update()
        {
            if (!isPaused)
            {
                if (tempo * nbsMetaData.tempo < 0)
                {
                    tickTimer -= Kernel.deltaTime * (nbsFile.tickTempo * 0.0005f) * tempo.Abs() * nbsMetaData.tempo;
                    while (tickTimer <= 0)
                    {
                        _tick--;
                        tickTimer += 0.05f;

                        SoundPlay();
                    }
                }
                else
                {
                    tickTimer += Kernel.deltaTime * (nbsFile.tickTempo * 0.0005f) * (tempo * nbsMetaData.tempo).Abs();
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



        bool allLayerLock;
        public void Refesh()
        {
            {
                if (!Kernel.isInitialLoadEnd)
                {
                    Remove();
                    return;
                }

                if (selectedNBSFile == null)
                {
                    soundData = ResourceManager.SearchNBSData(key, nameSpace);

                    if (soundData == null)
                    {
                        Remove();
                        return;
                    }
                    else if (soundData.sounds == null || soundData.sounds.Length <= 0)
                    {
                        Remove();
                        return;
                    }
                    else if (pitch == 0)
                    {
                        Remove();
                        return;
                    }
                }
                else
                    soundData = null;
            }

            if (!SoundManager.nbsList.Contains(this))
                SoundManager.nbsList.Add(this);

            {
                if (selectedNBSFile == null)
                {
                    loadedNBSFile = null;
                    nbsMetaData = soundData.sounds[Random.Range(0, soundData.sounds.Length)];
                }
                else
                {
                    loadedNBSFile = selectedNBSFile;
                    nbsMetaData = null;
                }
            }

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
                time = time;
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
                            SoundManager.PlaySound(blockType, "minecraft", volume * this.volume, false, pitch * this.pitch * nbsMetaData.pitch / Kernel.gameSpeed, 1, panStereo + this.panStereo, minDistance, maxDistance, transform);
                        else
                            SoundManager.PlaySound(blockType, "minecraft", volume * this.volume, false, pitch * this.pitch * nbsMetaData.pitch / Kernel.gameSpeed, 1, panStereo + this.panStereo);
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
            if (tick < nbsFile.loopStartTick || index >= nbsFile.nbsNotes.Count)
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

                        if (nbsFile.loopStartTick > 0)
                            _tick = nbsFile.loopStartTick;
                        else
                        {
                            _tick = 0;
                            _index = 0;
                        }
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
            tempo = 1;
            _index = 0;
            _tick = 0;

            SoundManager.nbsList.Remove(this);
            SoundPlayer[] soundObjects = GetComponentsInChildren<SoundPlayer>();
            for (int i = 0; i < soundObjects.Length; i++)
                soundObjects[i].Remove();
        }
    }
}