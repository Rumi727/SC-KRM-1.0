using Brigadier.NET;
using Brigadier.NET.Builder;
using Brigadier.NET.Exceptions;
using SCKRM.Renderer;
using SCKRM.Sound;
using System;
using TMPro;
using UnityEngine;

using BuiltInExceptions = SCKRM.Command.Exceptions.BuiltInExceptions;

namespace SCKRM.Command
{
    public sealed class CommandManager : Manager<CommandManager>
    {
        public static CommandDispatcher<DefaultCommandSource> commandDispatcher { get; } = new CommandDispatcher<DefaultCommandSource>();
        public static DefaultCommandSource defaultCommandSource { get; } = new DefaultCommandSource();

        [SerializeField] TMP_InputField _inputField; public TMP_InputField inputField => _inputField;

        void Awake()
        {
            if (SingletonCheck(this))
            {
                CommandSyntaxException.BuiltInExceptions = new BuiltInExceptions();
                CommandSyntaxException.ContextAmount = 32;

                RegisterGameSpeed();

                RegisterPlaySound();
                RegisterStopSound();
                RegisterStopSoundAll();

                RegisterPlayNBS();
                RegisterStopNBS();
                RegisterStopNBSAll();

                RegisterAllRefresh();
                RegisterAllRerender();
                RegisterAllTextRerender();
            }
        }

        static bool onValueChangedLock = false;
        public static void OnValueChanged(string input)
        {
            if (onValueChangedLock)
                return;

            onValueChangedLock = true;

            if (input.Contains("\n"))
            {
                instance.inputField.text = "";
                Execute(input.Replace("\n", ""));
            }

            onValueChangedLock = false;
        }

        public static void Execute(string input)
        {
            try
            {
                commandDispatcher.Execute(input, defaultCommandSource);
            }
            catch (CommandSyntaxException e)
            {
                Debug.LogException(e.GetCustomException());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        static void RegisterGameSpeed()
        {
            commandDispatcher.Register(x =>
                x.Literal("gamespeed")
                    .Then(x =>
                        x.Argument("value", Arguments.Float())
                            .Executes(x =>
                            {
                                Kernel.gameSpeed = Arguments.GetFloat(x, "value");

                                string log = CommandLanguage.SearchLanguage("change_to").Replace("%name%", nameof(Kernel.gameSpeed)).Replace("%value%", Kernel.gameSpeed.ToString());
                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                return 1;
                            })
                    )
                    .Executes(x =>
                    {
                        string log = CommandLanguage.SearchLanguage("value_of").Replace("%name%", nameof(Kernel.gameSpeed)).Replace("%value%", Kernel.gameSpeed.ToString());
                        x.Source.lastCommandResult = new CommandResult(true, Kernel.gameSpeed, Kernel.gameSpeed, log);
                        return (int)Kernel.gameSpeed;
                    })
            );
        }

        static void RegisterPlaySound()
        {
            commandDispatcher.Register(x =>
                x.Literal("playsound")
                    .Then(x =>
                        x.Argument("key", Arguments.NameSpacePathPair())
                            .Then(x =>
                                x.Argument("volume", Arguments.Float())
                                    .Then(x =>
                                        x.Argument("loop", Arguments.Bool())
                                            .Then(x =>
                                                x.Argument("pitch", Arguments.Float())
                                                    .Then(x =>
                                                        x.Argument("tempo", Arguments.Float())
                                                            .Then(x =>
                                                                x.Argument("panStereo", Arguments.Float(-1, 1))
                                                                    .Then(x =>
                                                                        x.Argument("minDistance", Arguments.Float(0))
                                                                            .Then(x =>
                                                                                x.Argument("maxDistance", Arguments.Float(0))
                                                                                    .Then(x =>
                                                                                        x.Argument("position", Arguments.Vector3())
                                                                                            .Executes(x =>
                                                                                            {
                                                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                                                float tempo = Arguments.GetFloat(x, "tempo");
                                                                                                float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                                float minDistance = Arguments.GetFloat(x, "minDistance");
                                                                                                float maxDistance = Arguments.GetFloat(x, "maxDistance");
                                                                                                Vector3 position = Arguments.GetVector3(x, "position");

                                                                                                SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance, maxDistance, null, position.x, position.y, position.z);

                                                                                                string log = CommandLanguage.SearchLanguage("play_sound");
                                                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                                return 1;
                                                                                            })
                                                                                    )
                                                                                    .Executes(x =>
                                                                                    {
                                                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                        float volume = Arguments.GetFloat(x, "volume");
                                                                                        bool loop = Arguments.GetBool(x, "loop");
                                                                                        float pitch = Arguments.GetFloat(x, "pitch");
                                                                                        float tempo = Arguments.GetFloat(x, "tempo");
                                                                                        float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                        float minDistance = Arguments.GetFloat(x, "minDistance");
                                                                                        float maxDistance = Arguments.GetFloat(x, "maxDistance");

                                                                                        SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance, maxDistance);

                                                                                        string log = CommandLanguage.SearchLanguage("play_sound");
                                                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                        return 1;
                                                                                    })
                                                                            )
                                                                            .Executes(x =>
                                                                            {
                                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                                float tempo = Arguments.GetFloat(x, "tempo");
                                                                                float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                float minDistance = Arguments.GetFloat(x, "minDistance");

                                                                                SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance);

                                                                                string log = CommandLanguage.SearchLanguage("play_sound");
                                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                return 1;
                                                                            })
                                                                    )
                                                                    .Executes(x =>
                                                                    {
                                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                        float volume = Arguments.GetFloat(x, "volume");
                                                                        bool loop = Arguments.GetBool(x, "loop");
                                                                        float pitch = Arguments.GetFloat(x, "pitch");
                                                                        float tempo = Arguments.GetFloat(x, "tempo");
                                                                        float panStereo = Arguments.GetFloat(x, "panStereo");

                                                                        SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo);

                                                                        string log = CommandLanguage.SearchLanguage("play_sound");
                                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                        return 1;
                                                                    })
                                                            )
                                                            .Executes(x =>
                                                            {
                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                float tempo = Arguments.GetFloat(x, "tempo");

                                                                SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo);

                                                                string log = CommandLanguage.SearchLanguage("play_sound");
                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                return 1;
                                                            })
                                                    )
                                                    .Executes(x =>
                                                    {
                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                        float volume = Arguments.GetFloat(x, "volume");
                                                        bool loop = Arguments.GetBool(x, "loop");
                                                        float pitch = Arguments.GetFloat(x, "pitch");

                                                        SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch);

                                                        string log = CommandLanguage.SearchLanguage("play_sound");
                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                        return 1;
                                                    })
                                            )
                                            .Executes(x =>
                                            {
                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                float volume = Arguments.GetFloat(x, "volume");
                                                bool loop = Arguments.GetBool(x, "loop");

                                                SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop);

                                                string log = CommandLanguage.SearchLanguage("play_sound");
                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                return 1;
                                            })
                                    )
                                    .Executes(x =>
                                    {
                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                        float volume = Arguments.GetFloat(x, "volume");

                                        SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume);

                                        string log = CommandLanguage.SearchLanguage("play_sound");
                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                        return 1;
                                    })
                            )
                            .Executes(x =>
                            {
                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                SoundManager.PlaySound(nameSpacePathPair.path, nameSpacePathPair.nameSpace);

                                string log = CommandLanguage.SearchLanguage("play_sound");
                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                return 1;
                            })
                    )
            );
        }

        static void RegisterStopSound()
        {
            commandDispatcher.Register(x =>
                x.Literal("stopsound")
                    .Then(x =>
                        x.Argument("key", Arguments.NameSpacePathPair())
                            .Then(x =>
                                x.Argument("all", Arguments.Bool())
                                    .Executes(x =>
                                    {
                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                        bool all = Arguments.GetBool(x, "all");
                                        int stopCount = SoundManager.StopSound(nameSpacePathPair.path, nameSpacePathPair.nameSpace, all);

                                        string log = CommandLanguage.SearchLanguage("stop_sound");
                                        x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                        return stopCount;
                                    })
                            )
                            .Executes(x =>
                            {
                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                int stopCount = SoundManager.StopSound(nameSpacePathPair.path, nameSpacePathPair.nameSpace);

                                string log = CommandLanguage.SearchLanguage("stop_sound");
                                x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                return stopCount;
                            })
                    )
            );
        }

        static void RegisterStopSoundAll()
        {
            commandDispatcher.Register(x =>
                x.Literal("stopsoundall")
                    .Then(x =>
                        x.Argument("bgm", Arguments.Bool())
                            .Executes(x =>
                            {
                                int stopCount = SoundManager.StopSoundAll(Arguments.GetBool(x, "bgm"));

                                string log = CommandLanguage.SearchLanguage("stop_sound_all");
                                x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                return stopCount;
                            })
                    )
                    .Executes(x =>
                    {
                        int stopCount = SoundManager.StopSoundAll();

                        string log = CommandLanguage.SearchLanguage("stop_sound_all");
                        x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                        return stopCount;
                    })
            );
        }

        static void RegisterPlayNBS()
        {
            commandDispatcher.Register(x =>
                x.Literal("playnbs")
                    .Then(x =>
                        x.Argument("key", Arguments.NameSpacePathPair())
                            .Then(x =>
                                x.Argument("volume", Arguments.Float())
                                    .Then(x =>
                                        x.Argument("loop", Arguments.Bool())
                                            .Then(x =>
                                                x.Argument("pitch", Arguments.Float())
                                                    .Then(x =>
                                                        x.Argument("tempo", Arguments.Float())
                                                            .Then(x =>
                                                                x.Argument("panStereo", Arguments.Float(-1, 1))
                                                                    .Then(x =>
                                                                        x.Argument("minDistance", Arguments.Float(0))
                                                                            .Then(x =>
                                                                                x.Argument("maxDistance", Arguments.Float(0))
                                                                                    .Then(x =>
                                                                                        x.Argument("position", Arguments.Vector3())
                                                                                            .Executes(x =>
                                                                                            {
                                                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                                                float tempo = Arguments.GetFloat(x, "tempo");
                                                                                                float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                                float minDistance = Arguments.GetFloat(x, "minDistance");
                                                                                                float maxDistance = Arguments.GetFloat(x, "maxDistance");
                                                                                                Vector3 position = Arguments.GetVector3(x, "position");

                                                                                                SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance, maxDistance, null, position.x, position.y, position.z);

                                                                                                string log = CommandLanguage.SearchLanguage("play_nbs");
                                                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                                return 1;
                                                                                            })
                                                                                    )
                                                                                    .Executes(x =>
                                                                                    {
                                                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                        float volume = Arguments.GetFloat(x, "volume");
                                                                                        bool loop = Arguments.GetBool(x, "loop");
                                                                                        float pitch = Arguments.GetFloat(x, "pitch");
                                                                                        float tempo = Arguments.GetFloat(x, "tempo");
                                                                                        float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                        float minDistance = Arguments.GetFloat(x, "minDistance");
                                                                                        float maxDistance = Arguments.GetFloat(x, "maxDistance");

                                                                                        SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance, maxDistance);

                                                                                        string log = CommandLanguage.SearchLanguage("play_nbs");
                                                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                        return 1;
                                                                                    })
                                                                            )
                                                                            .Executes(x =>
                                                                            {
                                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                                float tempo = Arguments.GetFloat(x, "tempo");
                                                                                float panStereo = Arguments.GetFloat(x, "panStereo");
                                                                                float minDistance = Arguments.GetFloat(x, "minDistance");

                                                                                SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo, minDistance);

                                                                                string log = CommandLanguage.SearchLanguage("play_nbs");
                                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                                return 1;
                                                                            })
                                                                    )
                                                                    .Executes(x =>
                                                                    {
                                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                        float volume = Arguments.GetFloat(x, "volume");
                                                                        bool loop = Arguments.GetBool(x, "loop");
                                                                        float pitch = Arguments.GetFloat(x, "pitch");
                                                                        float tempo = Arguments.GetFloat(x, "tempo");
                                                                        float panStereo = Arguments.GetFloat(x, "panStereo");

                                                                        SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo, panStereo);

                                                                        string log = CommandLanguage.SearchLanguage("play_nbs");
                                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                        return 1;
                                                                    })
                                                            )
                                                            .Executes(x =>
                                                            {
                                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                                float volume = Arguments.GetFloat(x, "volume");
                                                                bool loop = Arguments.GetBool(x, "loop");
                                                                float pitch = Arguments.GetFloat(x, "pitch");
                                                                float tempo = Arguments.GetFloat(x, "tempo");

                                                                SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch, tempo);

                                                                string log = CommandLanguage.SearchLanguage("play_nbs");
                                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                                return 1;
                                                            })
                                                    )
                                                    .Executes(x =>
                                                    {
                                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                        float volume = Arguments.GetFloat(x, "volume");
                                                        bool loop = Arguments.GetBool(x, "loop");
                                                        float pitch = Arguments.GetFloat(x, "pitch");

                                                        SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop, pitch);

                                                        string log = CommandLanguage.SearchLanguage("play_nbs");
                                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                        return 1;
                                                    })
                                            )
                                            .Executes(x =>
                                            {
                                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                                float volume = Arguments.GetFloat(x, "volume");
                                                bool loop = Arguments.GetBool(x, "loop");

                                                SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume, loop);

                                                string log = CommandLanguage.SearchLanguage("play_nbs");
                                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                                return 1;
                                            })
                                    )
                                    .Executes(x =>
                                    {
                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                        float volume = Arguments.GetFloat(x, "volume");

                                        SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, volume);

                                        string log = CommandLanguage.SearchLanguage("play_nbs");
                                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                        return 1;
                                    })
                            )
                            .Executes(x =>
                            {
                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                SoundManager.PlayNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace);

                                string log = CommandLanguage.SearchLanguage("play_nbs");
                                x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                                return 1;
                            })
                    )
            );
        }

        static void RegisterStopNBS()
        {
            commandDispatcher.Register(x =>
                x.Literal("stopnbs")
                    .Then(x =>
                        x.Argument("key", Arguments.NameSpacePathPair())
                            .Then(x =>
                                x.Argument("all", Arguments.Bool())
                                    .Executes(x =>
                                    {
                                        NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                        bool all = Arguments.GetBool(x, "all");
                                        int stopCount = SoundManager.StopNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace, all);

                                        string log = CommandLanguage.SearchLanguage("stop_nbs");
                                        x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                        return stopCount;
                                    })
                            )
                            .Executes(x =>
                            {
                                NameSpacePathPair nameSpacePathPair = Arguments.GetNameSpacePathPair(x, "key");
                                int stopCount = SoundManager.StopNBS(nameSpacePathPair.path, nameSpacePathPair.nameSpace);

                                string log = CommandLanguage.SearchLanguage("stop_nbs");
                                x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                return stopCount;
                            })
                    )
            );
        }

        static void RegisterStopNBSAll()
        {
            commandDispatcher.Register(x =>
                x.Literal("stopnbsall")
                    .Then(x =>
                        x.Argument("bgm", Arguments.Bool())
                            .Executes(x =>
                            {
                                int stopCount = SoundManager.StopNBSAll(Arguments.GetBool(x, "bgm"));

                                string log = CommandLanguage.SearchLanguage("stop_nbs_all");
                                x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                                return stopCount;
                            })
                    )
                    .Executes(x =>
                    {
                        int stopCount = SoundManager.StopNBSAll();

                        string log = CommandLanguage.SearchLanguage("stop_nbs_all");
                        x.Source.lastCommandResult = new CommandResult(true, stopCount, stopCount, log);
                        return SoundManager.StopNBSAll();
                    })
            );
        }

        static void RegisterAllRefresh()
        {
            commandDispatcher.Register(x =>
                x.Literal("allrefresh")
                    .Executes(x =>
                    {
                        Kernel.AllRefresh().Forget();

                        string log = CommandLanguage.SearchLanguage("all_refresh");
                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                        return 1;
                    })
            );
        }

        static void RegisterAllRerender()
        {
            commandDispatcher.Register(x =>
                x.Literal("allrenderer")
                    .Executes(x =>
                    {
                        RendererManager.AllRerender();

                        string log = CommandLanguage.SearchLanguage("all_rerender");
                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                        return 1;
                    })
            );
        }

        static void RegisterAllTextRerender()
        {
            commandDispatcher.Register(x =>
                x.Literal("alltextrenderer")
                    .Executes(x =>
                    {
                        RendererManager.AllTextRerender();

                        string log = CommandLanguage.SearchLanguage("all_text_rerender");
                        x.Source.lastCommandResult = new CommandResult(true, 1, 1, log);
                        return 1;
                    })
            );
        }
    }

    public class DefaultCommandSource
    {
        public CommandResult lastCommandResult { get; set; } = new CommandResult();
    }

    public struct CommandResult
    {
        public CommandResult(bool isSuccess = false, object result = null, double resultNumber = 0, string logText = "")
        {
            this.isSuccess = isSuccess;

            this.result = result;
            this.resultNumber = resultNumber;

            this.logText = logText;
        }

        public bool isSuccess { get; }

        public object result { get; }
        public double resultNumber { get; }

        public string logText { get; }
    }
}
