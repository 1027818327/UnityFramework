using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.Animations;
using UnityEngine.AI;
using UnityEngine.Audio;

namespace Tool
{
    using Object = UnityEngine.Object;

    public static class StripCode
    {
        private static readonly Dictionary<int, Type> YAMLClassMapping = new Dictionary<int, Type>()
        {
            { 1, typeof(GameObject)},
            { 2, typeof(Component)},
         // { 3, typeof(LevelGameManager)},
            { 4, typeof(Transform)},
         // { 5, typeof(TimeManager)},
         // { 6, typeof(GlobalGameManager)},
            { 8, typeof(Behaviour)},
         // { 9, typeof(GameManager)},
         // { 11, typeof(AudioManager)},
         // { 12, typeof(ParticleAnimator)},
         // { 13, typeof(InputManager)},
         // { 15, typeof(EllipsoidParticleEmitter)},
         // { 17, typeof(Pipeline)},
         // { 18, typeof(EditorExtension)},
         // { 19, typeof(Physics2DSettings)},
            { 20, typeof(Camera)},
            { 21, typeof(Material)},
            { 23, typeof(MeshRenderer)},
            { 25, typeof(Renderer)},
         // { 26, typeof(ParticleRenderer)},
            { 27, typeof(Texture)},
            { 28, typeof(Texture2D)},
         // { 29, typeof(SceneSettings)},
            { 30, typeof(GraphicsSettings)},
            { 33, typeof(MeshFilter)},
            { 41, typeof(OcclusionPortal)},
            { 43, typeof(Mesh)},
            { 45, typeof(Skybox)},
            { 47, typeof(QualitySettings)},
            { 48, typeof(Shader)},
            { 49, typeof(TextAsset)},
            { 50, typeof(Rigidbody2D)},
         // { 51, typeof(Physics2DManager)},
            { 53, typeof(Collider2D)},
            { 54, typeof(Rigidbody)},
         // { 55, typeof(PhysicsManager)},
            { 56, typeof(Collider)},
            { 57, typeof(Joint)},
            { 58, typeof(CircleCollider2D)},
            { 59, typeof(HingeJoint)},
            { 60, typeof(PolygonCollider2D)},
            { 61, typeof(BoxCollider2D)},
            { 62, typeof(PhysicsMaterial2D)},
            { 64, typeof(MeshCollider)},
            { 65, typeof(BoxCollider)},
         // { 66, typeof(SpriteCollider2D)},
            { 68, typeof(EdgeCollider2D)},
            { 72, typeof(ComputeShader)},
            { 74, typeof(AnimationClip)},
            { 75, typeof(ConstantForce)},
         // { 76, typeof(WorldParticleCollider)},
         // { 78, typeof(TagManager)},
            { 81, typeof(AudioListener)},
            { 82, typeof(AudioSource)},
            { 83, typeof(AudioClip)},
            { 84, typeof(RenderTexture)},
         // { 87, typeof(MeshParticleEmitter)},
         // { 88, typeof(ParticleEmitter)},
            { 89, typeof(Cubemap)},
            { 90, typeof(Avatar)},
            { 91, typeof(AnimatorController )},
#if UNITY_5
            { 92, typeof(GUILayer)},
#endif
            { 93, typeof(RuntimeAnimatorController)},
         // { 94, typeof(ScriptMapper)},
            { 95, typeof(Animator)},
            { 96, typeof(TrailRenderer)},
         // { 98, typeof(DelayedCallManager)},
            { 102, typeof(TextMesh)},
            { 104, typeof(RenderSettings)},
            { 108, typeof(Light)},
         // { 109, typeof(CGProgram)},
         // { 110, typeof(BaseAnimationTrack)},
            { 111, typeof(UnityEngine.Animation)},
            { 114, typeof(MonoBehaviour)},
            { 115, typeof(MonoScript)},
         // { 116, typeof(MonoManager)},
            { 117, typeof(Texture3D)},
         // { 118, typeof(NewAnimationTrack)},
            { 119, typeof(Projector)},
            { 120, typeof(LineRenderer)},
            { 121, typeof(Flare)},
         // { 122, typeof(Halo)},
            { 123, typeof(LensFlare)},
            { 124, typeof(FlareLayer)},
         // { 125, typeof(HaloLayer)},
         // { 126, typeof(NavMeshAreas)},
         // { 127, typeof(HaloManager)},
            { 128, typeof(Font)},
            { 129, typeof(PlayerSettings)},
         // { 130, typeof(NamedObject)},
#if UNITY_5
            { 131, typeof(GUITexture)},
            { 132, typeof(GUIText)},
#endif
            { 133, typeof(GUIElement)},
            { 134, typeof(PhysicMaterial)},
            { 135, typeof(SphereCollider)},
            { 136, typeof(CapsuleCollider)},
            { 137, typeof(SkinnedMeshRenderer)},
            { 138, typeof(FixedJoint)},
         // { 140, typeof(RaycastCollider)},
         // { 141, typeof(BuildSettings)},
            { 142, typeof(UnityEngine.AssetBundle)},
            { 143, typeof(CharacterController)},
            { 144, typeof(CharacterJoint)},
            { 145, typeof(SpringJoint)},
            { 146, typeof(WheelCollider)},
         // { 147, typeof(ResourceManager)},
#if UNITY_5
            { 148, typeof(NetworkView)},
#endif
         // { 149, typeof(NetworkManager)},
         // { 150, typeof(PreloadData)},
            { 152, typeof(MovieTexture)},
            { 153, typeof(ConfigurableJoint)},
            { 154, typeof(TerrainCollider)},
         // { 155, typeof(MasterServerInterface)},
            { 156, typeof(TerrainData)},
            { 157, typeof(LightmapSettings)},
            { 158, typeof(WebCamTexture)},
            { 159, typeof(EditorSettings)},
         // { 160, typeof(InteractiveCloth)},
         // { 161, typeof(ClothRenderer)},
            { 162, typeof(EditorUserSettings)},
         // { 163, typeof(SkinnedCloth)},
            { 164, typeof(AudioReverbFilter)},
            { 165, typeof(AudioHighPassFilter)},
            { 166, typeof(AudioChorusFilter)},
            { 167, typeof(AudioReverbZone)},
            { 168, typeof(AudioEchoFilter)},
            { 169, typeof(AudioLowPassFilter)},
            { 170, typeof(AudioDistortionFilter)},
            { 171, typeof(SparseTexture)},
            { 180, typeof(AudioBehaviour)},
         // { 181, typeof(AudioFilter)},
            { 182, typeof(WindZone)},
            { 183, typeof(Cloth)},
            { 184, typeof(SubstanceArchive)},
#if UNITY_5
            { 185, typeof(ProceduralMaterial)},
            { 186, typeof(ProceduralTexture)},
#endif
            { 191, typeof(OffMeshLink)},
            { 192, typeof(OcclusionArea)},
            { 193, typeof(Tree)},
         // { 194, typeof(NavMeshObsolete)},
            { 195, typeof(NavMeshAgent)},
         // { 196, typeof(NavMeshSettings)},
         // { 197, typeof(LightProbesLegacy)},
            { 198, typeof(ParticleSystem)},
            { 199, typeof(ParticleSystemRenderer)},
            { 200, typeof(ShaderVariantCollection)},
            { 205, typeof(LODGroup)},
            { 206, typeof(BlendTree)},
            { 207, typeof(Motion)},
            { 208, typeof(NavMeshObstacle)},
         // { 210, typeof(TerrainInstance)},
            { 212, typeof(SpriteRenderer)},
            { 213, typeof(Sprite)},
         // { 214, typeof(CachedSpriteAtlas)},
            { 215, typeof(ReflectionProbe)},
         // { 216, typeof(ReflectionProbes)},
            { 218, typeof(Terrain)},
            { 220, typeof(LightProbeGroup)},
            { 221, typeof(AnimatorOverrideController)},
            { 222, typeof(CanvasRenderer)},
            { 223, typeof(Canvas)},
            { 224, typeof(RectTransform)},
            { 225, typeof(CanvasGroup)},
            { 226, typeof(BillboardAsset)},
            { 227, typeof(BillboardRenderer)},
         // { 228, typeof(SpeedTreeWindAsset)},
            { 229, typeof(AnchoredJoint2D)},
            { 230, typeof(Joint2D)},
            { 231, typeof(SpringJoint2D)},
            { 232, typeof(DistanceJoint2D)},
            { 233, typeof(HingeJoint2D)},
            { 234, typeof(SliderJoint2D)},
            { 235, typeof(WheelJoint2D)},
         // { 238, typeof(NavMeshData)},
            { 240, typeof(AudioMixer)},
         // { 241, typeof(AudioMixerController)},
         // { 243, typeof(AudioMixerGroupController)},
         // { 244, typeof(AudioMixerEffectController)},
         // { 245, typeof(AudioMixerSnapshotController)},
            { 246, typeof(PhysicsUpdateBehaviour2D)},
            { 247, typeof(ConstantForce2D)},
            { 248, typeof(Effector2D)},
            { 249, typeof(AreaEffector2D)},
            { 250, typeof(PointEffector2D)},
            { 251, typeof(PlatformEffector2D)},
            { 252, typeof(SurfaceEffector2D)},
            { 258, typeof(LightProbes)},
         // { 271, typeof(SampleClip)},
            { 272, typeof(AudioMixerSnapshot)},
            { 273, typeof(AudioMixerGroup)},
            { 290, typeof(AssetBundleManifest)},
         // { 1001, typeof(Prefab)},
         // { 1002, typeof(EditorExtensionImpl)},
            { 1003, typeof(AssetImporter)},
            { 1004, typeof(AssetDatabase)},
         // { 1005, typeof(Mesh3DSImporter)},
            { 1006, typeof(TextureImporter)},
            { 1007, typeof(ShaderImporter)},
         // { 1008, typeof(ComputeShaderImporter)},
            { 1011, typeof(AvatarMask)},
            { 1020, typeof(AudioImporter)},
         // { 1026, typeof(HierarchyState)},
         // { 1027, typeof(GUIDSerializer)},
         // { 1028, typeof(AssetMetaData)},
            { 1029, typeof(DefaultAsset)},
         // { 1030, typeof(DefaultImporter)},
         // { 1031, typeof(TextScriptImporter)},
            { 1032, typeof(SceneAsset)},
         // { 1034, typeof(NativeFormatImporter)},
            { 1035, typeof(MonoImporter)},
         // { 1037, typeof(AssetServerCache)},
         // { 1038, typeof(LibraryAssetImporter)},
            { 1040, typeof(ModelImporter)},
         // { 1041, typeof(FBXImporter)},
            { 1042, typeof(TrueTypeFontImporter)},
            { 1044, typeof(MovieImporter)},
            { 1045, typeof(EditorBuildSettings)},
         //{ 1046, typeof(DDSImporter)},
         // { 1048, typeof(InspectorExpandedState)},
         // { 1049, typeof(AnnotationManager)},
            { 1050, typeof(PluginImporter)},
            { 1051, typeof(EditorUserBuildSettings)},
         // { 1052, typeof(PVRImporter)},
         // { 1053, typeof(ASTCImporter)},
         // { 1054, typeof(KTXImporter)},
            { 1101, typeof(AnimatorStateTransition)},
            { 1102, typeof(AnimatorState)},
            { 1105, typeof(HumanTemplate)},
            { 1107, typeof(AnimatorStateMachine)},
         // { 1108, typeof(PreviewAssetType)},
            { 1109, typeof(AnimatorTransition)},
            { 1110, typeof(SpeedTreeImporter)},
            { 1111, typeof(AnimatorTransitionBase)},
            { 1112, typeof(SubstanceImporter)},
            { 1113, typeof(LightmapParameters)},
         // { 1120, typeof(LightmapSnapshot)},
        };

        private const string YAMLMagicString = "--- !u!";
        private const string YAMLMagicString2 = "!u!";
        private const string YAMLFileFilter = "t:GameObject t:AnimatorController t:Material t:scene t:ScriptableObject";
        private const string EditorDllPrefix = "UnityEditor";
        private const string EngineDllPrefix = "UnityEngine";
        private const string UGUIDllPrefix = "UnityEngine.UI";

        private static readonly char[] Spliters = new char[1] { ' ' };
        // 这个修改为项目AssetBundle打包目录
        private static readonly string[] SearchPaths = new string[1]
        {
            "Assets",
        };
        private static readonly string OutputFilePath = Application.persistentDataPath + "/CodeStripWhiteList.txt";
        private static Type[] UGUITypes = Assembly.GetAssembly(typeof(UnityEngine.UI.Image)).GetTypes();

        private static HashSet<string> UnityClasses = new HashSet<string>();
        private static HashSet<string> UGUIClasses = new HashSet<string>();
        private static HashSet<int> MissingClasses = new HashSet<int>();

        [MenuItem("ILRuntime群福利/生成裁剪白名单", false, 0)]
        public static void BuildStripCodeWhiteList()
        {
            UnityClasses.Clear();
            UGUIClasses.Clear();
            MissingClasses.Clear();

            string[] assetGUIDs = AssetDatabase.FindAssets(YAMLFileFilter, SearchPaths);
            int assetCount = assetGUIDs.Length;
            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
                if (i % 43 == 0)
                    EditorUtility.DisplayProgressBar("Processing...", string.Format("{0}/{1} {2}", i + 1, assetCount, assetPath), (i + 1) / (float)assetCount);

                // Check UGUI Component
                GameObject prefab = AssetDatabase.LoadAssetAtPath<Object>(assetPath) as GameObject;
                if (prefab != null)
                {
                    Component[] components = prefab.GetComponentsInChildren<Component>(true);
                    if (components != null && components.Length > 0)
                    {
                        for (int j = 0; j < components.Length; j++)
                        {
                            if (components[j] != null)
                            {
                                Type type = components[j].GetType();
                                if (UGUITypes.Contains(type))
                                {
                                    UGUIClasses.Add(type.FullName);
                                }
                            }
                            else
                            {
                                Debug.LogWarning("[CodeStripUtil] The prefab has NULL component:" + assetPath);
                            }
                        }
                    }
                }

                // Check Engine Class
                string[] lines = File.ReadAllLines(assetPath);
                for (int j = 0; j < lines.Length; j++)
                {
                    string line = lines[j];
                    if (line.StartsWith(YAMLMagicString))
                    {
                        int classID = int.Parse(line.Split(Spliters)[1].Replace(YAMLMagicString2, string.Empty));
                        Type className;
                        if (!YAMLClassMapping.TryGetValue(classID, out className))
                        {
                            MissingClasses.Add(classID);
                        }
                        else
                        {
                            UnityClasses.Add(className.FullName);
                        }
                    }
                }
            }

            EditorUtility.ClearProgressBar();

            List<string> unityClasses = new List<string>(UnityClasses);
            unityClasses.Sort();
            List<string> editorClasses = new List<string>();
            List<string> engineClasses = new List<string>();

            for (int i = 0; i < unityClasses.Count; i++)
            {
                string className = unityClasses[i];
                if (className.StartsWith(EngineDllPrefix))
                {
                    engineClasses.Add(className);
                }
                else if (className.StartsWith(EditorDllPrefix))
                {
                    editorClasses.Add(className);
                }
            }

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine("===============UnityEngine===============");
            foreach (var line in engineClasses)
            {
                strBuilder.AppendLine(line);
            }
            strBuilder.AppendLine("\n==============UnityEngine.UI=============");
            foreach (var line in UGUIClasses)
            {
                strBuilder.AppendLine(line);
            }
            strBuilder.AppendLine("\n===============UnityEditor===============");
            foreach (var line in editorClasses)
            {
                strBuilder.AppendLine(line);
            }

            try
            {
                File.WriteAllText(OutputFilePath, strBuilder.ToString());
                System.Diagnostics.Process.Start(OutputFilePath);
            }
            catch (Exception ex)
            {
                Debug.Log("[CodeStripUtil] Write File Error:" + ex.ToString());
            }
        }
    }
}
