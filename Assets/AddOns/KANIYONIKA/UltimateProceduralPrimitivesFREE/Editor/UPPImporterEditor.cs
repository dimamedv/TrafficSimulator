using UnityEngine;
using UnityEditor;
#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace UltimateProceduralPrimitivesFREE
{

  [CustomEditor(typeof(UPPImporter))]
  sealed class UPPImporterEditor : ScriptedImporterEditor
  {
    SerializedProperty _shape;
    SerializedProperty _plane;
    SerializedProperty _planeFlex;
    SerializedProperty _planeSuperEllipse;
    SerializedProperty _box;
    SerializedProperty _boxFlex;
    SerializedProperty _boxRounded;
    SerializedProperty _pyramid;
    SerializedProperty _pyramidFlex;
    SerializedProperty _pyramidPerfectTriangularFlex;
    SerializedProperty _boxSuperEllipsoid;
    SerializedProperty _sphere;
    SerializedProperty _sphereIco;
    SerializedProperty _sphereFibonacci;
    SerializedProperty _tearDrop;
    SerializedProperty _cylinder;
    SerializedProperty _cone;
    SerializedProperty _supershape;

    SerializedProperty _otherShapes;


    bool _isOpenAboutShape = false;
    bool _isOpenAboutVertices = false;


    public override void OnEnable()
    {
      base.OnEnable();
      _shape = serializedObject.FindProperty("_shape");
      _plane = serializedObject.FindProperty("_plane");
      _planeFlex = serializedObject.FindProperty("_planeFlex");
      _planeSuperEllipse = serializedObject.FindProperty("_planeSuperEllipse");
      _box = serializedObject.FindProperty("_box");
      _boxFlex = serializedObject.FindProperty("_boxFlex");
      _boxRounded = serializedObject.FindProperty("_boxRounded");
      _boxSuperEllipsoid = serializedObject.FindProperty("_boxSuperEllipsoid");
      _pyramid = serializedObject.FindProperty("_pyramid");
      _pyramidFlex = serializedObject.FindProperty("_pyramidFlex");
      _pyramidPerfectTriangularFlex = serializedObject.FindProperty("_pyramidPerfectTriangularFlex");
      _sphere = serializedObject.FindProperty("_sphere");
      _sphereIco = serializedObject.FindProperty("_sphereIco");
      _sphereFibonacci = serializedObject.FindProperty("_sphereFibonacci");
      _tearDrop = serializedObject.FindProperty("_tearDrop");
      _cylinder = serializedObject.FindProperty("_cylinder");
      _cone = serializedObject.FindProperty("_cone");
      _supershape = serializedObject.FindProperty("_supershape");

      _otherShapes = serializedObject.FindProperty("_otherShapes");

    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();

      EditorGUILayout.PropertyField(_shape);

      switch ((Shape)_shape.intValue)
      {
        case Shape.Plane: EditorGUILayout.PropertyField(_plane); break;
        // case Shape.PlaneFlex: EditorGUILayout.PropertyField(_planeFlex); break;
        // case Shape.PlaneSuperEllipse: EditorGUILayout.PropertyField(_planeSuperEllipse); break;
        case Shape.Box: EditorGUILayout.PropertyField(_box); break;
        // case Shape.BoxFlex: EditorGUILayout.PropertyField(_boxFlex); break;
        case Shape.BoxRounded: EditorGUILayout.PropertyField(_boxRounded); break;
        // case Shape.BoxSuperEllipsoid: EditorGUILayout.PropertyField(_boxSuperEllipsoid); break;
        case Shape.Pyramid: EditorGUILayout.PropertyField(_pyramid); break;
        // case Shape.PyramidFlex: EditorGUILayout.PropertyField(_pyramidFlex); break;
        // case Shape.PyramidPerfectTriangularFlex: EditorGUILayout.PropertyField(_pyramidPerfectTriangularFlex); break;
        case Shape.Sphere: EditorGUILayout.PropertyField(_sphere); break;
        // case Shape.SphereIco: EditorGUILayout.PropertyField(_sphereIco); break;
        // case Shape.SphereFibonacci: EditorGUILayout.PropertyField(_sphereFibonacci); break;
        // case Shape.TearDrop: EditorGUILayout.PropertyField(_tearDrop); break;
        // case Shape.Cylinder: EditorGUILayout.PropertyField(_cylinder); break;
        case Shape.Cone: EditorGUILayout.PropertyField(_cone); break;
        case Shape.Supershape: EditorGUILayout.PropertyField(_supershape); break;

        case Shape.OtherShapes: EditorGUILayout.PropertyField(_otherShapes); break;

      }

      serializedObject.ApplyModifiedProperties();
      ApplyRevertGUI();


      // HelpBox
      string helpMsg = "";
      switch ((Shape)_shape.intValue)
      {
        case Shape.Plane:
          helpMsg = "\n"
                    + "This is a basic Plane. If you want to change the orientation, use Orientation or Direction. SurfaceType is meaningless because this is a basic Plane.\n"
                    + "-----------------------\n"
                    + "?????????????????????Plane??????????????????????????????????????????Orientation???Direction????????????????????????????????????????????????Plane????????????SurfaceType??????????????????????????????\n"
                    + "";
          break;
        // case Shape.PlaneFlex:
        //   helpMsg = "\n"
        //             + "FlexPlane is a plane where each vertex can be moved. To change it, change the value of VertexOffsets, which can be set for each orientation. If you want to change the orientation, use Orientation or Direction. The operation is a little difficult, but you will get used to it.\n"
        //             + "-----------------------\n"
        //             + "FlexPlane????????????????????????????????????????????????Plane??????????????????????????????VertexOffsets????????????????????????????????????VertexOffsets???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
        //             + "";
        //   break;
        // case Shape.PlaneSuperEllipse:
        //   helpMsg = "\n"
        //             + "SuperEllipsePlane is an ellipse-shaped Plane. The roundness of each vertex can be adjusted by changing the values of N1, N2, N3, and N4. Values must be between 0.0 and 1.0 or greater than 1.0.\n"
        //             + "-----------------------\n"
        //             + "SuperEllipsePlane???????????????Plane????????? N1???N2???N3???N4?????????????????????????????????????????????????????????????????????????????????????????????0.0??????1.0????????????1.0????????????????????????????????????\n"
        //             + "";
        //   break;
        case Shape.Box:
          helpMsg = "\n"
                    + "This is a basic Box. Note that SurfaceType is meaningless for basic Box.\n"
                    + "-----------------------\n"
                    + "???????????????????????????????????????????????????????????????Plane????????????SurfaceType??????????????????????????????\n"
                    + "";
          break;
        // case Shape.BoxFlex:
        //   helpMsg = "\n"
        //             + "FlexBox is a box that allows each vertex to be moved. To change it, change the value of VertexOffsets. The operation is a little difficult, but you will get used to it.\n"
        //             + "-----------------------\n"
        //             + "FlexBox????????????????????????????????????????????????Box??????????????????????????????VertexOffsets?????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
        //             + "";
        //   break;
        case Shape.BoxRounded:
          helpMsg = "\n"
                    + "RoundedBox is used to create a rounded box. If you change the value of Radius, you will see that the corners and edges are given a rounded appearance. For more beautiful roundness, use BoxSuperEllipsoid. Note: When you make the Box itself larger, make sure to increase the Radius value accordingly.\n"
                    + "-----------------------\n"
                    + "RoundedBox???????????????????????????????????????????????????????????????????????????Radius???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????BoxSuperEllipsoid???????????????????????????????????????Box??????????????????????????????????????????????????????Radius???????????????????????????????????????????????????????????????????????????\n"
                    + "";
          break;
        // case Shape.BoxSuperEllipsoid:
        //   helpMsg = "\n"
        //             + "SuperEllipsoid is upward compatible with RoundedBox. By adjusting the N1 and N2 parameters, the up-down rounding and side rounding can be set. The N value should be between 0.0 and 1.0, or greater than 1.0.\n"
        //             + "-----------------------\n"
        //             + "SuperEllipsoid???RoundedBox???????????????????????? N1???N2????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????N?????????0.0??????1.0????????????1.0????????????????????????????????????\n"
        //             + "";
        //   break;
        case Shape.Pyramid:
          helpMsg = "\n"
                    + "This is a Pyramid. When used with Y_Axis, it is recommended to set PivotPosition to Down.\n"
                    + "\n"
                    + "?????????Pyramid????????? Y_Axis????????????????????????PivotPosition???Down??????????????????????????????????????????\n"
                    + "";
          break;
        // case Shape.PyramidFlex:
        //   helpMsg = "\n"
        //             + "FlexPyramid is a pyramid that allows each vertex to be moved. To change it, change the value of VertexOffsets. The operation is a little difficult, but you will get used to it.\n"
        //             + "-----------------------\n"
        //             + "FlexPyramid????????????????????????????????????????????????Pyramid????????? ?????????????????????VertexOffsets?????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
        //             + "";
        //   break;
        // case Shape.PyramidPerfectTriangularFlex:
        //   helpMsg = "\n"
        //             + "FlexPerfectTriangularPyramid is a pyramid with perfect equilateral triangles on 3 sides. Set the length of one side in the Length parameter.  In addition, each vertex can be moved. To change it, change the value of VertexOffsets. The operation is a little difficult, but you will get used to it.\n"
        //             + "-----------------------\n"
        //             + "FlexPerfectTriangularPyramid??????????????????????????????3???????????????????????????????????? Length??????????????????????????????????????????????????????????????? ???????????????????????????????????????????????????????????? ?????????????????????VertexOffsets???????????????????????????????????? ?????????????????????????????????????????????????????????????????????\n"
        //             + "";
        //   break;
        case Shape.Sphere:
          helpMsg = "\n"
                    + "This is a UVSphere.\n"
                    + "- If you want to create a vertical or horizontal ellipsoid mesh, use SuperEllipsoidBox and set the parameters N1 and N2 to 1, and the values of Height, Width or Depth to your liking.\n"
                    + "- If you want to create a symmetrical, equilateral triangular polyhedron, use Icosphere. It is a more uniform sphere than the UV sphere.\n"
                    + "- If you want to create a sphere with triangles(vertices, edges, and faces) spread evenly, please use FibonacciSphere. The Fibonacci sequence is used to draw vertices in a spiral. It is suitable for using shaders or further editing vertices because the vertices are easy to handle.\n"
                    + "-----------------------\n"
                    + "?????????UV???????????????\n"
                    + "????????????????????????????????????????????????????????????????????????SuperEllipsoidBox??????????????????????????????N1???N2???1????????????Height???Width?????????Depth??????????????????????????????????????????\n"
                    + "??????????????????????????????????????????????????????????????????Icosphere????????????????????????UV????????????????????????????????????????????????\n"
                    + "??????????????????????????????????????????????????????????????????????????????????????????FibonacciSphere???????????????????????????FibonacciSphere???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
                    + "";
          break;
        // case Shape.SphereIco:
        //   helpMsg = "\n"
        //             + "*** Caution ***\n"
        //             + "It is dangerous to set Subdivision to 6 or higher. If you have the machine power, it may not be a problem.\n"
        //             + "This is an ICO sphere.\n"
        //             + "- It is a symmetrical, equilateral triangular polyhedron.\n"
        //             + "- It is more uniform than the UV sphere\n"
        //             + "- Some UVs are defective due to lack of vertices in the structure.\n"
        //             + "-----------------------\n"
        //             + "*** ??? ??? ***\n"
        //             + "Subdivision??????6????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
        //             + "?????????ICO????????????\n"
        //             + "???????????????????????????????????????????????????\n"
        //             + "???UV????????????????????????????????????????????????\n"
        //             + "???????????????????????????????????????UV???????????????????????????????????????\n"
        //             + "";
        //   break;
        // case Shape.SphereFibonacci:
        //   helpMsg = "\n"
        //             + "This is an Fibonacci Sphere.\n"
        //             + "- FibonacciSphere is a sphere with triangles (vertices, edges, and faces) spread evenly.\n"
        //             + "- The Fibonacci sequence is used to draw vertices in a spiral.\n"
        //             + "- It is suitable for using shaders or further editing vertices because the vertices are easy to handle. (Set to Smooth.)\n"
        //             + "- Some UVs are defective due to lack of vertices in the structure.\n"
        //             + "-----------------------\n"
        //             + "??????????????????????????????????????????\n"
        //             + "???FibonacciSphere???????????????(??????????????????)???????????????????????????????????????\n"
        //             + "?????????????????????????????????????????????????????????????????????????????????\n"
        //             + "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????Smooth?????????????????????????????? \n"
        //             + "???????????????????????????????????????UV???????????????????????????????????????\n"
        //             + "";
        //   break;
        // case Shape.TearDrop:
        //   helpMsg = "\n"
        //             + "This is a Tear Drop\n"
        //             + "-----------------------\n"
        //             + "????????????????????????\n"
        //             + "";
        //   break;
        // case Shape.Cylinder:
        //   helpMsg = "\n"
        //             + "This is a Cylinder. TopRadius and BottomRadius can be used to change the size of the circle. You can also create pentagons and hexagons by setting SurfaceType to Flat and changing Columns. The parameter Caps determines whether TopRadius and BottomRadius are closed or not.\n"
        //             + "-----------------------\n"
        //             + "????????????????????????????????? TopRadius???BottomRadius????????????????????????????????????????????????????????? SurfaceType???Flat?????????Columns???????????????????????????????????????????????????????????????????????????????????? ?????????????????????Caps???TopRadius???BottomRadius?????????????????????????????????\n"
        //             + "";
        //   break;
        case Shape.Cone:
          helpMsg = "\n"
                    + "Cone is made from Cylinder. The parameters are the same as for Cylinder.\n"
                    + "-----------------------\n"
                    + "Cone???Cylinder???????????????????????????????????????????????????Cylinder??????????????????\n"
                    + "";
          break;
        case Shape.Supershape:
          helpMsg = "\n"
                    + "Supershape is a mesh that consists of a very complex mathematical formula. The parameter Radius is the radius of the mesh and should be considered simply as the size of the mesh. The other parameters N1, N2, N3, M, A, and B affect the shape of the mesh.\n"
                    + "\n"
                    + "It is almost impossible to predict how each parameter will affect the shape. We recommend that you actually try to change the parameters.\n"
                    + "\n"
                    + "First, try leaving Segment at 100, setting both M to 0 and all other parameters to 1 and Apply. You should now have a perfect sphere. From there, try changing the value of M to 5 or the value of A. You will get the idea step by step.\n"
                    + "\n"
                    + "Again, it is impossible to predict how these parameters in Supershape will affect the shape. A Google search for 'Supershape' may help you find interesting shapes discovered by our predecessors.\n"
                    + "\n"
                    + "If you find interesting parameter values, please let us know via email or in the review section. If you like, we would be happy to register it as a Preset of UltimateProceduralPrimitives.\n"
                    + "-----------------------\n"
                    + "Supershape???????????????????????????????????????Mesh????????? ??????????????????Radius???Mesh????????????????????????????????????????????????????????????????????????????????? ????????????????????????????????? N1???N2???N3???M???A???B ??? Mesh?????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "??????????????????????????????????????????????????????????????????????????????????????????????????????????????? ?????????????????????????????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "????????????Segment???100?????????????????????M???????????????0?????????????????????????????????????????????1?????????Apply?????????????????????????????? ????????????????????????????????????????????? ????????????M?????????5?????????????????????A????????????????????????????????????????????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "?????????????????????????????????Supershape??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? Google??????Supershape????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "?????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????? ?????????????????????UltimateProceduralPrimitives??? Preset ???????????????????????????????????????????????????????????????\n"
                    + "";
          break;
      }

      string aboutVertexMsg = "\n"
                    + "Note on using vertices.\n"
                    + "There are the following differences depending on whether SurfaceType is set to \"Smooth\" or \"Flat\".\n"
                    + "\n"
                    + "Smooth: The value returned by \"mesh.vertices\" is the vertex information calculated from the formula.There are essentially no duplicate vertices, and they are regularly aligned.\n"
                    + "\n"
                    + "Flat: The value returned by \"mesh.vertices\" is the triangle information created based on the vertex information calculated from the formula.Since FlatSurface cannot be realized if vertices are shared, duplicate vertex information is added to create triangles. Ordering regularity is not guaranteed.\n"
                    + "\n"
                    + "From the above, for example, if you want to use vertices in the order of the Fibonacci formula, set Smooth.\n"
                    + "Note that triangle information can be obtained by \"mesh.GetIndexCount(0)\".\n"
                    + "-----------------------\n"
                    + "????????????????????????????????????\n"
                    + "SurfaceType??????Smooth??????????????????Flat??????????????????????????????????????????????????????\n"
                    + "\n"
                    + "Smooth:???mesh.vertices????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "Flat:???mesh.vertices??????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????FlatSurface???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
                    + "\n"
                    + "?????????????????????????????????????????????????????????????????????????????????????????????Smooth???????????????????????????\n"
                    + "?????????????????????????????????mesh.GetIndexCount(0)???????????????????????????\n"
                    + "";


      bool isOpenAboutShape = EditorGUILayout.Foldout(_isOpenAboutShape, "About This Shape");
      bool isOpenAboutVertices = EditorGUILayout.Foldout(_isOpenAboutVertices, "About Vertices");

      if (_isOpenAboutShape != isOpenAboutShape)
        _isOpenAboutShape = isOpenAboutShape;
      if (_isOpenAboutVertices != isOpenAboutVertices)
        _isOpenAboutVertices = isOpenAboutVertices;

      EditorGUILayout.BeginVertical(GUI.skin.box);

      ///
      /// THIS PROGRAM IS IN ONLY FREE VERSION --START--
      ///
      string freeVersionMsg = "\n"
      + "As the paid version is updated, we plan to release more Shapes that can be used in the free version.\n"
      + "You can help us keep our asset continuously updated by promoting UltimateProceduralPrimitives and giving us good ratings and reviews.\n"
      + "\n"
      + "The full version is available for purchase here\n"
      + "\n"
      + "https://assetstore.unity.com/packages/3d/characters/ultimate-procedural-primitives-225557?aid=1011lpBbi\n"
      + "-----------------------\n"
      + "???????????????????????????????????????????????????????????????????????????Shape????????????????????????????????????\n"
      + "UltimateProceduralPrimitives????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????\n"
      + "\n"
      + "???????????????????????????????????????????????????????????????\n"
      + "\n"
      + "https://assetstore.unity.com/packages/3d/characters/ultimate-procedural-primitives-225557?aid=1011lpBbi\n"
      + "";
      switch ((Shape)_shape.intValue)
      {
        case Shape.OtherShapes:
          EditorGUILayout.HelpBox(freeVersionMsg, MessageType.Info);
          break;
      }
      ///
      /// THIS PROGRAM IS IN ONLY FREE VERSION --END--
      ///

      //?????????????????????GUI??????
      if (isOpenAboutShape)
      { EditorGUILayout.HelpBox(helpMsg, MessageType.Info); }

      if (isOpenAboutVertices)
      { EditorGUILayout.HelpBox(aboutVertexMsg, MessageType.Info); }

      EditorGUILayout.EndVertical();

    }


    [MenuItem("Assets/Create/UltimateProceduralPrimitives")]
    public static void CreateNewAsset()
      => ProjectWindowUtil.CreateAssetWithContent("New UltimateProceduralPrimitive.upp", "");
  }
}
