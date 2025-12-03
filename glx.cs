using Gaucho;
using Gtk;
using OpenTK.Graphics.OpenGL4;
public static  class Glx
{
 // Gambas module file

 // Copyright (C) Ing Martin P Cristia
 //
 // This program is free software; you can redistribute it and/or modify
 // it under the terms of the GNU General public static License as published by
 // the Free Software Foundation; either version 3 of the License, or
 // (at your option) any later version.
 //
 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General public static License for more details.
 //
 // You should have received a copy of the GNU General public static License
 // along with this program; if not, write to the Free Software
 // Foundation, Inc., 51 Franklin St, Fifth Floor,
 // Boston, MA  02110-1301  USA

 // El mundo segun OpenGL
 //       +-------------------------------------------------------------------------------------------+
 //       | 0,0                                     |top         x  P2 x,y                            |
 //       |                                         |           /                                     |
 //       |                                         |          /                                      |
 //       |                                         |         /                                       |
 //       |                                         |        /                                        |
 //       |                                         |       x P1 x,y                                  |
 //       |left                                     |                                           rigth | = glOrtho(left, rigth, bottom, top)
 //       |-----------------------------------------+-------------------------------------------------|   el papel donde voy a dibujar
 //       |                xxxxxxxxxxx              |0,0                                              |   = metros o lo que sea
 //       |                xxxxxxxxxxx              |                                                 |   Establece la ProjectionMatrix
 //       |                xxxx0,0xxxx              |                                                 |   y no se la toca mas
 //       |                xxxxxxxxxxx              |                                                 |
 //       |                xxxxxxxxxxx              |                                                 |
 //       |                                         |                                                 |
 //       |                     |<------------------|                                                 |
 //       |             ModelMatrix>Translate       |                                                 |
 //       |                                         |bottom                                           |gldrwArea.w,.h = GL.Viewport = el area donde dibujo
 //       +-------------------------------------------------------------------------------------------+           = pixeles
 //Fast 
 // una lbreria de funciones para pasar de Paint a OpenGL
public static bool Initialized = false;
public static LFFFonts ActualFont ;          // the class
public static LFFFonts UnicodeFont ;         
public static string ActualFontName = "romanc";           // the name
public static double ActualFontHeigth = 1;                 // the letter heigth
public static double FontScale = 0.1125;                      // the general scale factor

public static double zLevel = 0;

public static List<string> FontsNameList =[];          // lista de fuentes LFF disponibles ya cargadas

public static int[] FontsCAllLists =[];          // listas de listas de caracteres

public static  Dictionary<string, LFFFonts> glFont =[];         
public  struct TextureSt
{
    public static string FileName ="";
    public static string TextureName ="";
    public static int Id ;
    public static Image? hImage ;
}

public static  TextureSt[] glTextures =[];
public  struct GLColorSt
{

    double r ;
    double g ;
    double b ;
    double Alpha ;

}

public static  float[] CurrentColor = new float[4] {1.0f, 1.0f, 1.0f, 1.0f} ;          // current color RGBA
 // A shader is a small C program that the GPU understands
 // minimal shaders we need to compile at the GPU


public static double escalaGL ;         

public static int ViewportWIdth ;         
public static int ViewportHeight ;         

public static  int[] hText =[];         

 // new OpenGL stuff
public static  int[]                        GLDrwList =[];          // all entities, each one
public static  int[]                 GLDrwListEditing =[];          // all entities on edit by some tool, including new ones

 // lineas de puntos
public static  int[] LineStipples =[];         
public static  int[] LineStippleScales ;         

 // modo inmediato o programado
public static bool InmediateMode = true;
public static bool InmediateModeRequired ;         
public static bool VBO_present = false;    // si tenemos VBO que dibujar
  
 //
 // Copyright (C) Ing Martin P Cristia
 //
 // This program is free software; you can redistribute it and/or modify
 // it under the terms of the GNU General public static License as published by
 // the Free Software Foundation; either version 3 of the License, or
 // (at your option) any later version.
 //
 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General public static License for more details.
 //
 // You should have received a copy of the GNU General public static License
 // along with this program; if not, write to the Free Software
 // Foundation, Inc., 51 Franklin St, Fifth Floor,
 // Boston, MA  02110-1301  USA


 // WARNING: double en Gambas = DOUBLE en GL
 //          SINGLE en Gambas = double en GL

public const int DEPTH_BUFFER_BIT = 0x000000000100;
public const int STENCIL_BUFFER_BIT = 0x000000000400;
public const int COLOR_BUFFER_BIT = 0x000000004000;
public const int FALSE = 0;
public const int TRUE = 1;
public const int POINTS = 0x00000000;
public const int LINES = 0x00000001;
public const int LINE_LOOP = 0x00000002;
public const int LINE_STRIP = 0x00000003;
public const int TRIANGLES = 0x00000004;
public const int TRIANGLE_STRIP = 0x00000005;
public const int TRIANGLE_FAN = 0x00000006;
public const int QUADS = 0x00000007;
public const int NEVER = 0x00000200;
public const int LESS = 0x00000201;
public const int EQUAL = 0x00000202;
public const int LEQUAL = 0x00000203;
public const int GREATER = 0x00000204;
public const int NOTEQUAL = 0x00000205;
public const int GEQUAL = 0x00000206;
public const int ALWAYS = 0x00000207;
public const int ZERO = 0;
public const int ONE = 1;
public const int SRC_COLOR = 0x00000300;
public const int ONE_MINUS_SRC_COLOR = 0x00000301;
public const int SRC_ALPHA = 0x00000302;
public const int ONE_MINUS_SRC_ALPHA = 0x00000303;
public const int DST_ALPHA = 0x00000304;
public const int ONE_MINUS_DST_ALPHA = 0x00000305;
public const int DST_COLOR = 0x00000306;
public const int ONE_MINUS_DST_COLOR = 0x00000307;
public const int SRC_ALPHA_SATURATE = 0x00000308;
public const int NONE = 0;
public const int FRONT_LEFT = 0x00000400;
public const int FRONT_RIGHT = 0x00000401;
public const int BACK_LEFT = 0x00000402;
public const int BACK_RIGHT = 0x00000403;
public const int FRONT = 0x00000404;
public const int BACK = 0x00000405;
public const int LEFT = 0x00000406;
public const int RIGHT = 0x00000407;
public const int FRONT_AND_BACK = 0x00000408;
public const int NO_ERROR = 0;
public const int INVALID_ENUM = 0x00000500;
public const int INVALID_VALUE = 0x00000501;
public const int INVALID_OPERATION = 0x00000502;
public const int OUT_OF_MEMORY = 0x00000505;
public const int CW = 0x00000900;
public const int CCW = 0x00000901;
public const int POINT_SIZE = 0x00000B11;
public const int POINT_SIZE_RANGE = 0x00000B12;
public const int POINT_SIZE_GRANULARITY = 0x00000B13;
public const int LINE_SMOOTH = 0x00000B20;
public const int LINE_WIDTH = 0x00000B21;
public const int LINE_WIDTH_RANGE = 0x00000B22;
public const int LINE_WIDTH_GRANULARITY = 0x00000B23;
public const int POLYGON_MODE = 0x00000B40;
public const int POLYGON_SMOOTH = 0x00000B41;
public const int CULL_FACE = 0x00000B44;
public const int CULL_FACE_MODE = 0x00000B45;
public const int FRONT_FACE = 0x00000B46;
public const int DEPTH_RANGE = 0x00000B70;
public const int DEPTH_TEST = 0x00000B71;
public const int DEPTH_WRITEMASK = 0x00000B72;
public const int DEPTH_CLEAR_VALUE = 0x00000B73;
public const int DEPTH_FUNC = 0x00000B74;
public const int STENCIL_TEST = 0x00000B90;
public const int STENCIL_CLEAR_VALUE = 0x00000B91;
public const int STENCIL_FUNC = 0x00000B92;
public const int STENCIL_VALUE_MASK = 0x00000B93;
public const int STENCIL_FAIL = 0x00000B94;
public const int STENCIL_PASS_DEPTH_FAIL = 0x00000B95;
public const int STENCIL_PASS_DEPTH_PASS = 0x00000B96;
public const int STENCIL_REF = 0x00000B97;
public const int STENCIL_WRITEMASK = 0x00000B98;
public const int VIEWPORT = 0x00000BA2;
public const int DITHER = 0x00000BD0;
public const int BLEND_DST = 0x00000BE0;
public const int BLEND_SRC = 0x00000BE1;
public const int BLEND = 0x00000BE2;
public const int LOGIC_OP_MODE = 0x00000BF0;
public const int DRAW_BUFFER = 0x00000C01;
public const int READ_BUFFER = 0x00000C02;
public const int SCISSOR_BOX = 0x00000C10;
public const int SCISSOR_TEST = 0x00000C11;
public const int COLOR_CLEAR_VALUE = 0x00000C22;
public const int COLOR_WRITEMASK = 0x00000C23;
public const int DOUBLEBUFFER = 0x00000C32;
public const int STEREO = 0x00000C33;
public const int LINE_SMOOTH_HINT = 0x00000C52;
public const int POLYGON_SMOOTH_HINT = 0x00000C53;
public const int UNPACK_SWAP_BYTES = 0x00000CF0;
public const int UNPACK_LSB_FIRST = 0x00000CF1;
public const int UNPACK_ROW_LENGTH = 0x00000CF2;
public const int UNPACK_SKIP_ROWS = 0x00000CF3;
public const int UNPACK_SKIP_PIXELS = 0x00000CF4;
public const int UNPACK_ALIGNMENT = 0x00000CF5;
public const int PACK_SWAP_BYTES = 0x00000D00;
public const int PACK_LSB_FIRST = 0x00000D01;
public const int PACK_ROW_LENGTH = 0x00000D02;
public const int PACK_SKIP_ROWS = 0x00000D03;
public const int PACK_SKIP_PIXELS = 0x00000D04;
public const int PACK_ALIGNMENT = 0x00000D05;
public const int MAX_TEXTURE_SIZE = 0x00000D33;
public const int MAX_VIEWPORT_DIMS = 0x00000D3A;
public const int SUBPIXEL_BITS = 0x00000D50;
public const int TEXTURE_1D = 0x00000DE0;
public const int TEXTURE_2D = 0x00000DE1;
public const int TEXTURE_WIDTH = 0x00001000;
public const int TEXTURE_HEIGHT = 0x00001001;
public const int TEXTURE_BORDER_COLOR = 0x00001004;
public const int DONT_CARE = 0x00001100;
public const int FASTEST = 0x00001101;
public const int NICEST = 0x00001102;
public const int BYTE = 0x00001400;
public const int UNSIGNED_BYTE = 0x00001401;
public const int SHORT = 0x00001402;
public const int UNSIGNED_SHORT = 0x00001403;
public const int INT = 0x00001404;
public const int UNSIGNED_INT = 0x00001405;
public const int FLOAT  = 0x00001406;
public const int STACK_OVERFLOW = 0x00000503;
public const int STACK_UNDERFLOW = 0x00000504;
public const int CLEAR = 0x00001500;
public const int AND = 0x00001501;
public const int AND_REVERSE = 0x00001502;
public const int COPY = 0x00001503;
public const int AND_INVERTED = 0x00001504;
public const int NOOP = 0x00001505;
public const int XOR = 0x00001506;
public const int OR = 0x00001507;
public const int NOR = 0x00001508;
public const int EQUIV = 0x00001509;
public const int INVERT = 0x0000150A;
public const int OR_REVERSE = 0x0000150B;
public const int COPY_INVERTED = 0x0000150C;
public const int OR_INVERTED = 0x0000150D;
public const int NAND = 0x0000150E;
public const int SET = 0x0000150F;
public const int TEXTURE = 0x00001702;
public const int COLOR = 0x00001800;
public const int DEPTH = 0x00001801;
public const int STENCIL = 0x00001802;
public const int STENCIL_INDEX = 0x00001901;
public const int DEPTH_COMPONENT = 0x00001902;
public const int RED = 0x00001903;
public const int GREEN = 0x00001904;
public const int BLUE = 0x00001905;
public const int ALPHA = 0x00001906;
public const int RGB = 0x00001907;
public const int RGBA = 0x00001908;
public const int POINT = 0x00001B00;
public const int LINE = 0x00001B01;
public const int FILL = 0x00001B02;
public const int KEEP = 0x00001E00;
public const int REPLACE = 0x00001E01;
public const int INCR = 0x00001E02;
public const int DECR = 0x00001E03;
public const int VENDOR = 0x00001F00;
public const int RENDERER = 0x00001F01;
public const int VERSION = 0x00001F02;
public const int EXTENSIONS = 0x00001F03;
public const int NEAREST = 0x00002600;
public const int LINEAR = 0x00002601;
public const int NEAREST_MIPMAP_NEAREST = 0x00002700;
public const int LINEAR_MIPMAP_NEAREST = 0x00002701;
public const int NEAREST_MIPMAP_LINEAR = 0x00002702;
public const int LINEAR_MIPMAP_LINEAR = 0x00002703;
public const int TEXTURE_MAG_FILTER = 0x00002800;
public const int TEXTURE_MIN_FILTER = 0x00002801;
public const int TEXTURE_WRAP_S = 0x00002802;
public const int TEXTURE_WRAP_T = 0x00002803;
public const int REPEAT = 0x00002901;

public const int COLOR_LOGIC_OP = 0x00000BF2;
public const int POLYGON_OFFSET_UNITS = 0x00002A00;
public const int POLYGON_OFFSET_POINT = 0x00002A01;
public const int POLYGON_OFFSET_LINE = 0x00002A02;
public const int POLYGON_OFFSET_FILL = 0x00008037;
public const int POLYGON_OFFSET_FACTOR = 0x00008038;
public const int TEXTURE_BINDING_1D = 0x00008068;
public const int TEXTURE_BINDING_2D = 0x00008069;
public const int TEXTURE_INTERNAL_FORMAT = 0x00001003;
public const int TEXTURE_RED_SIZE = 0x0000805C;
public const int TEXTURE_GREEN_SIZE = 0x0000805D;
public const int TEXTURE_BLUE_SIZE = 0x0000805E;
public const int TEXTURE_ALPHA_SIZE = 0x0000805F;
public const int DOUBLE = 0x0000140A;
public const int PROXY_TEXTURE_1D = 0x00008063;
public const int PROXY_TEXTURE_2D = 0x00008064;
public const int R3_G3_B2 = 0x00002A10;
public const int RGB4 = 0x0000804F;
public const int RGB5 = 0x00008050;
public const int RGB8 = 0x00008051;
public const int RGB10 = 0x00008052;
public const int RGB12 = 0x00008053;
public const int RGB16 = 0x00008054;
public const int RGBA2 = 0x00008055;
public const int RGBA4 = 0x00008056;
public const int RGB5_A1 = 0x00008057;
public const int RGBA8 = 0x00008058;
public const int RGB10_A2 = 0x00008059;
public const int RGBA12 = 0x0000805A;
public const int RGBA16 = 0x0000805B;
public const int VERTEX_ARRAY = 0x00008074;
public const int NORMAL_ARRAY = 0x00008075;
public const int COLOR_ARRAY = 0x00008076;
public const int INDEX_ARRAY = 0x00008077;
public const int TEXTURE_COORD_ARRAY = 0x00008078;
public const int EDGE_FLAG_ARRAY = 0x00008079;

public const int UNSIGNED_BYTE_3_3_2 = 0x00008032;
public const int UNSIGNED_SHORT_4_4_4_4 = 0x00008033;
public const int UNSIGNED_SHORT_5_5_5_1 = 0x00008034;
public const int UNSIGNED_INT_8_8_8_8 = 0x00008035;
public const int UNSIGNED_INT_10_10_10_2 = 0x00008036;
public const int TEXTURE_BINDING_3D = 0x0000806A;
public const int PACK_SKIP_IMAGES = 0x0000806B;
public const int PACK_IMAGE_HEIGHT = 0x0000806C;
public const int UNPACK_SKIP_IMAGES = 0x0000806D;
public const int UNPACK_IMAGE_HEIGHT = 0x0000806E;
public const int TEXTURE_3D = 0x0000806F;
public const int PROXY_TEXTURE_3D = 0x00008070;
public const int TEXTURE_DEPTH = 0x00008071;
public const int TEXTURE_WRAP_R = 0x00008072;
public const int MAX_3D_TEXTURE_SIZE = 0x00008073;
public const int UNSIGNED_BYTE_2_3_3_REV = 0x00008362;
public const int UNSIGNED_SHORT_5_6_5 = 0x00008363;
public const int UNSIGNED_SHORT_5_6_5_REV = 0x00008364;
public const int UNSIGNED_SHORT_4_4_4_4_REV = 0x00008365;
public const int UNSIGNED_SHORT_1_5_5_5_REV = 0x00008366;
public const int UNSIGNED_INT_8_8_8_8_REV = 0x00008367;
public const int UNSIGNED_INT_2_10_10_10_REV = 0x00008368;
public const int BGR = 0x000080E0;
public const int BGRA = 0x000080E1;
public const int MAX_ELEMENTS_VERTICES = 0x000080E8;
public const int MAX_ELEMENTS_INDICES = 0x000080E9;
public const int CLAMP_TO_EDGE = 0x0000812F;
public const int TEXTURE_MIN_LOD = 0x0000813A;
public const int TEXTURE_MAX_LOD = 0x0000813B;
public const int TEXTURE_BASE_LEVEL = 0x0000813C;
public const int TEXTURE_MAX_LEVEL = 0x0000813D;
public const int SMOOTH_POINT_SIZE_RANGE = 0x00000B12;
public const int SMOOTH_POINT_SIZE_GRANULARITY = 0x00000B13;
public const int SMOOTH_LINE_WIDTH_RANGE = 0x00000B22;
public const int SMOOTH_LINE_WIDTH_GRANULARITY = 0x00000B23;
public const int ALIASED_LINE_WIDTH_RANGE = 0x0000846E;
public const int TEXTURE0 = 0x000084C0;
public const int TEXTURE1 = 0x000084C1;
public const int TEXTURE2 = 0x000084C2;
public const int TEXTURE3 = 0x000084C3;
public const int TEXTURE4 = 0x000084C4;
public const int TEXTURE5 = 0x000084C5;
public const int TEXTURE6 = 0x000084C6;
public const int TEXTURE7 = 0x000084C7;
public const int TEXTURE8 = 0x000084C8;
public const int TEXTURE9 = 0x000084C9;
public const int TEXTURE10 = 0x000084CA;
public const int TEXTURE11 = 0x000084CB;
public const int TEXTURE12 = 0x000084CC;
public const int TEXTURE13 = 0x000084CD;
public const int TEXTURE14 = 0x000084CE;
public const int TEXTURE15 = 0x000084CF;
public const int TEXTURE16 = 0x000084D0;
public const int TEXTURE17 = 0x000084D1;
public const int TEXTURE18 = 0x000084D2;
public const int TEXTURE19 = 0x000084D3;
public const int TEXTURE20 = 0x000084D4;
public const int TEXTURE21 = 0x000084D5;
public const int TEXTURE22 = 0x000084D6;
public const int TEXTURE23 = 0x000084D7;
public const int TEXTURE24 = 0x000084D8;
public const int TEXTURE25 = 0x000084D9;
public const int TEXTURE26 = 0x000084DA;
public const int TEXTURE27 = 0x000084DB;
public const int TEXTURE28 = 0x000084DC;
public const int TEXTURE29 = 0x000084DD;
public const int TEXTURE30 = 0x000084DE;
public const int TEXTURE31 = 0x000084DF;
public const int ACTIVE_TEXTURE = 0x000084E0;
public const int MULTISAMPLE = 0x0000809D;
public const int SAMPLE_ALPHA_TO_COVERAGE = 0x0000809E;
public const int SAMPLE_ALPHA_TO_ONE = 0x0000809F;
public const int SAMPLE_COVERAGE = 0x000080A0;
public const int SAMPLE_BUFFERS = 0x000080A8;
public const int SAMPLES = 0x000080A9;
public const int SAMPLE_COVERAGE_VALUE = 0x000080AA;
public const int SAMPLE_COVERAGE_INVERT = 0x000080AB;
public const int TEXTURE_CUBE_MAP = 0x00008513;
public const int TEXTURE_BINDING_CUBE_MAP = 0x00008514;
public const int TEXTURE_CUBE_MAP_POSITIVE_X = 0x00008515;
public const int TEXTURE_CUBE_MAP_NEGATIVE_X = 0x00008516;
public const int TEXTURE_CUBE_MAP_POSITIVE_Y = 0x00008517;
public const int TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x00008518;
public const int TEXTURE_CUBE_MAP_POSITIVE_Z = 0x00008519;
public const int TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x0000851A;
public const int PROXY_TEXTURE_CUBE_MAP = 0x0000851B;
public const int MAX_CUBE_MAP_TEXTURE_SIZE = 0x0000851C;
public const int COMPRESSED_RGB = 0x000084ED;
public const int COMPRESSED_RGBA = 0x000084EE;
public const int TEXTURE_COMPRESSION_HINT = 0x000084EF;
public const int TEXTURE_COMPRESSED_IMAGE_SIZE = 0x000086A0;
public const int TEXTURE_COMPRESSED = 0x000086A1;
public const int NUM_COMPRESSED_TEXTURE_FORMATS = 0x000086A2;
public const int COMPRESSED_TEXTURE_FORMATS = 0x000086A3;
public const int CLAMP_TO_BORDER = 0x0000812D;

public const int BLEND_DST_RGB = 0x000080C8;
public const int BLEND_SRC_RGB = 0x000080C9;
public const int BLEND_DST_ALPHA = 0x000080CA;
public const int BLEND_SRC_ALPHA = 0x000080CB;
public const int POINT_FADE_THRESHOLD_SIZE = 0x00008128;
public const int DEPTH_COMPONENT16 = 0x000081A5;
public const int DEPTH_COMPONENT24 = 0x000081A6;
public const int DEPTH_COMPONENT32 = 0x000081A7;
public const int MIRRORED_REPEAT = 0x00008370;
public const int MAX_TEXTURE_LOD_BIAS = 0x000084FD;
public const int TEXTURE_LOD_BIAS = 0x00008501;
public const int INCR_WRAP = 0x00008507;
public const int DECR_WRAP = 0x00008508;
public const int TEXTURE_DEPTH_SIZE = 0x0000884A;
public const int TEXTURE_COMPARE_MODE = 0x0000884C;
public const int TEXTURE_COMPARE_FUNC = 0x0000884D;
public const int BLEND_COLOR = 0x00008005;
public const int BLEND_EQUATION = 0x00008009;
public const int CONSTANT_COLOR = 0x00008001;
public const int ONE_MINUS_CONSTANT_COLOR = 0x00008002;
public const int CONSTANT_ALPHA = 0x00008003;
public const int ONE_MINUS_CONSTANT_ALPHA  = 0x00008004;
public const int FUNC_ADD = 0x00008006;
public const int FUNC_REVERSE_SUBTRACT = 0x0000800B;
public const int FUNC_SUBTRACT = 0x0000800A;
public const int MIN = 0x00008007;
public const int MAX = 0x00008008;
public const int BUFFER_SIZE = 0x00008764;
public const int BUFFER_USAGE = 0x00008765;
public const int QUERY_COUNTER_BITS = 0x00008864;
public const int CURRENT_QUERY = 0x00008865;
public const int QUERY_RESULT = 0x00008866;
public const int QUERY_RESULT_AVAILABLE = 0x00008867;
public const int ARRAY_BUFFER = 0x00008892;
public const int ELEMENT_ARRAY_BUFFER = 0x00008893;
public const int ARRAY_BUFFER_BINDING = 0x00008894;
public const int ELEMENT_ARRAY_BUFFER_BINDING = 0x00008895;
public const int VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x0000889F;
public const int READ_ONLY = 0x000088B8;
public const int WRITE_ONLY = 0x000088B9;
public const int READ_WRITE = 0x000088BA;
public const int BUFFER_ACCESS = 0x000088BB;
public const int BUFFER_MAPPED = 0x000088BC;
public const int BUFFER_MAP_POINTER = 0x000088BD;
public const int STREAM_DRAW = 0x000088E0;
public const int STREAM_READ = 0x000088E1;
public const int STREAM_COPY = 0x000088E2;
public const int STATIC_DRAW = 0x000088E4;
public const int STATIC_READ = 0x000088E5;
public const int STATIC_COPY = 0x000088E6;
public const int DYNAMIC_DRAW = 0x000088E8;
public const int DYNAMIC_READ = 0x000088E9;
public const int DYNAMIC_COPY = 0x000088EA;
public const int SAMPLES_PASSED = 0x00008914;
public const int SRC1_ALPHA = 0x00008589;
public const int BLEND_EQUATION_RGB = 0x00008009;
public const int VERTEX_ATTRIB_ARRAY_ENABLED = 0x00008622;
public const int VERTEX_ATTRIB_ARRAY_SIZE = 0x00008623;
public const int VERTEX_ATTRIB_ARRAY_STRIDE = 0x00008624;
public const int VERTEX_ATTRIB_ARRAY_TYPE = 0x00008625;
public const int CURRENT_VERTEX_ATTRIB = 0x00008626;
public const int VERTEX_PROGRAM_POINT_SIZE = 0x00008642;
public const int VERTEX_ATTRIB_ARRAY_POINTER = 0x00008645;
public const int STENCIL_BACK_FUNC = 0x00008800;
public const int STENCIL_BACK_FAIL = 0x00008801;
public const int STENCIL_BACK_PASS_DEPTH_FAIL = 0x00008802;
public const int STENCIL_BACK_PASS_DEPTH_PASS = 0x00008803;
public const int MAX_DRAW_BUFFERS = 0x00008824;
public const int DRAW_BUFFER0 = 0x00008825;
public const int DRAW_BUFFER1 = 0x00008826;
public const int DRAW_BUFFER2 = 0x00008827;
public const int DRAW_BUFFER3 = 0x00008828;
public const int DRAW_BUFFER4 = 0x00008829;
public const int DRAW_BUFFER5 = 0x0000882A;
public const int DRAW_BUFFER6 = 0x0000882B;
public const int DRAW_BUFFER7 = 0x0000882C;
public const int DRAW_BUFFER8 = 0x0000882D;
public const int DRAW_BUFFER9 = 0x0000882E;
public const int DRAW_BUFFER10 = 0x0000882F;
public const int DRAW_BUFFER11 = 0x00008830;
public const int DRAW_BUFFER12 = 0x00008831;
public const int DRAW_BUFFER13 = 0x00008832;
public const int DRAW_BUFFER14 = 0x00008833;
public const int DRAW_BUFFER15 = 0x00008834;
public const int BLEND_EQUATION_ALPHA = 0x0000883D;
public const int MAX_VERTEX_ATTRIBS = 0x00008869;
public const int VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x0000886A;
public const int MAX_TEXTURE_IMAGE_UNITS = 0x00008872;
public const int FRAGMENT_SHADER = 0x00008B30;
public const int VERTEX_SHADER = 0x00008B31;
public const int MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x00008B49;
public const int MAX_VERTEX_UNIFORM_COMPONENTS = 0x00008B4A;
public const int MAX_VARYING_singleS = 0x00008B4B;
public const int MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x00008B4C;
public const int MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x00008B4D;
public const int SHADER_TYPE = 0x00008B4F;
public const int single_VEC2 = 0x00008B50;
public const int single_VEC3 = 0x00008B51;
public const int single_VEC4 = 0x00008B52;
public const int INT_VEC2 = 0x00008B53;
public const int INT_VEC3 = 0x00008B54;
public const int INT_VEC4 = 0x00008B55;
public const int BOOL = 0x00008B56;
public const int BOOL_VEC2 = 0x00008B57;
public const int BOOL_VEC3 = 0x00008B58;
public const int BOOL_VEC4 = 0x00008B59;
public const int single_MAT2 = 0x00008B5A;
public const int single_MAT3 = 0x00008B5B;
public const int single_MAT4 = 0x00008B5C;
public const int SAMPLER_1D = 0x00008B5D;
public const int SAMPLER_2D = 0x00008B5E;
public const int SAMPLER_3D = 0x00008B5F;
public const int SAMPLER_CUBE = 0x00008B60;
public const int SAMPLER_1D_SHADOW = 0x00008B61;
public const int SAMPLER_2D_SHADOW = 0x00008B62;
public const int DELETE_STATUS = 0x00008B80;
public const int COMPILE_STATUS = 0x00008B81;
public const int LINK_STATUS = 0x00008B82;
public const int VALIDATE_STATUS = 0x00008B83;
public const int INFO_LOG_LENGTH = 0x00008B84;
public const int ATTACHED_SHADERS = 0x00008B85;
public const int ACTIVE_UNIFORMS = 0x00008B86;
public const int ACTIVE_UNIFORM_MAX_LENGTH = 0x00008B87;
public const int SHADER_SOURCE_LENGTH = 0x00008B88;
public const int ACTIVE_ATTRIBUTES = 0x00008B89;
public const int ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x00008B8A;
public const int FRAGMENT_SHADER_DERIVATIVE_HINT = 0x00008B8B;
public const int SHADING_LANGUAGE_VERSION = 0x00008B8C;
public const int CURRENT_PROGRAM = 0x00008B8D;
public const int POINT_SPRITE_COORD_ORIGIN = 0x00008CA0;
public const int LOWER_LEFT = 0x00008CA1;
public const int UPPER_LEFT = 0x00008CA2;
public const int STENCIL_BACK_REF = 0x00008CA3;
public const int STENCIL_BACK_VALUE_MASK = 0x00008CA4;
public const int STENCIL_BACK_WRITEMASK = 0x00008CA5;
public const int PIXEL_PACK_BUFFER = 0x000088EB;
public const int PIXEL_UNPACK_BUFFER = 0x000088EC;
public const int PIXEL_PACK_BUFFER_BINDING = 0x000088ED;
public const int PIXEL_UNPACK_BUFFER_BINDING = 0x000088EF;
public const int single_MAT2x3 = 0x00008B65;
public const int single_MAT2x4 = 0x00008B66;
public const int single_MAT3x2 = 0x00008B67;
public const int single_MAT3x4 = 0x00008B68;
public const int single_MAT4x2 = 0x00008B69;
public const int single_MAT4x3 = 0x00008B6A;
public const int SRGB = 0x00008C40;
public const int SRGB8 = 0x00008C41;
public const int SRGB_ALPHA = 0x00008C42;
public const int SRGB8_ALPHA8 = 0x00008C43;
public const int COMPRESSED_SRGB = 0x00008C48;
public const int COMPRESSED_SRGB_ALPHA = 0x00008C49;
public const int COMPARE_REF_TO_TEXTURE = 0x0000884E;
public const int CLIP_DISTANCE0 = 0x00003000;
public const int CLIP_DISTANCE1 = 0x00003001;
public const int CLIP_DISTANCE2 = 0x00003002;
public const int CLIP_DISTANCE3 = 0x00003003;
public const int CLIP_DISTANCE4 = 0x00003004;
public const int CLIP_DISTANCE5 = 0x00003005;
public const int CLIP_DISTANCE6 = 0x00003006;
public const int CLIP_DISTANCE7 = 0x00003007;
public const int MAX_CLIP_DISTANCES = 0x00000D32;
public const int MAJOR_VERSION = 0x0000821B;
public const int MINOR_VERSION = 0x0000821C;
public const int NUM_EXTENSIONS = 0x0000821D;
public const int CONTEXT_FLAGS = 0x0000821E;
public const int COMPRESSED_RED = 0x00008225;
public const int COMPRESSED_RG = 0x00008226;
public const int CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x000000000001;
public const int RGBA32F = 0x00008814;
public const int RGB32F = 0x00008815;
public const int RGBA16F = 0x0000881A;
public const int RGB16F = 0x0000881B;
public const int VERTEX_ATTRIB_ARRAY_int = 0x000088FD;
public const int MAX_ARRAY_TEXTURE_LAYERS = 0x000088FF;
public const int MIN_PROGRAM_TEXEL_OFFSET = 0x00008904;
public const int MAX_PROGRAM_TEXEL_OFFSET = 0x00008905;
public const int CLAMP_READ_COLOR = 0x0000891C;
public const int FIXED_ONLY = 0x0000891D;
public const int MAX_VARYING_COMPONENTS = 0x00008B4B;
public const int TEXTURE_1D_ARRAY = 0x00008C18;
public const int PROXY_TEXTURE_1D_ARRAY = 0x00008C19;
public const int TEXTURE_2D_ARRAY = 0x00008C1A;
public const int PROXY_TEXTURE_2D_ARRAY = 0x00008C1B;
public const int TEXTURE_BINDING_1D_ARRAY = 0x00008C1C;
public const int TEXTURE_BINDING_2D_ARRAY = 0x00008C1D;
public const int R11F_G11F_B10F = 0x00008C3A;
public const int UNSIGNED_INT_10F_11F_11F_REV = 0x00008C3B;
public const int RGB9_E5 = 0x00008C3D;
public const int UNSIGNED_INT_5_9_9_9_REV = 0x00008C3E;
public const int TEXTURE_SHARED_SIZE = 0x00008C3F;
public const int TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x00008C76;
public const int TRANSFORM_FEEDBACK_BUFFER_MODE = 0x00008C7F;
public const int MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x00008C80;
public const int TRANSFORM_FEEDBACK_VARYINGS = 0x00008C83;
public const int TRANSFORM_FEEDBACK_BUFFER_START = 0x00008C84;
public const int TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x00008C85;
public const int PRIMITIVES_GENERATED = 0x00008C87;
public const int TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x00008C88;
public const int RASTERIZER_DISCARD = 0x00008C89;
public const int MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x00008C8A;
public const int MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x00008C8B;
public const int INTERLEAVED_ATTRIBS = 0x00008C8C;
public const int SEPARATE_ATTRIBS = 0x00008C8D;
public const int TRANSFORM_FEEDBACK_BUFFER = 0x00008C8E;
public const int TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x00008C8F;
public const int RGBA32UI = 0x00008D70;
public const int RGB32UI = 0x00008D71;
public const int RGBA16UI = 0x00008D76;
public const int RGB16UI = 0x00008D77;
public const int RGBA8UI = 0x00008D7C;
public const int RGB8UI = 0x00008D7D;
public const int RGBA32I = 0x00008D82;
public const int RGB32I = 0x00008D83;
public const int RGBA16I = 0x00008D88;
public const int RGB16I = 0x00008D89;
public const int RGBA8I = 0x00008D8E;
public const int RGB8I = 0x00008D8F;
public const int RED_int = 0x00008D94;
public const int GREEN_int = 0x00008D95;
public const int BLUE_int = 0x00008D96;
public const int RGB_int = 0x00008D98;
public const int RGBA_int = 0x00008D99;
public const int BGR_int = 0x00008D9A;
public const int BGRA_int = 0x00008D9B;
public const int SAMPLER_1D_ARRAY = 0x00008DC0;
public const int SAMPLER_2D_ARRAY = 0x00008DC1;
public const int SAMPLER_1D_ARRAY_SHADOW = 0x00008DC3;
public const int SAMPLER_2D_ARRAY_SHADOW = 0x00008DC4;
public const int SAMPLER_CUBE_SHADOW = 0x00008DC5;
public const int UNSIGNED_INT_VEC2 = 0x00008DC6;
public const int UNSIGNED_INT_VEC3 = 0x00008DC7;
public const int UNSIGNED_INT_VEC4 = 0x00008DC8;
public const int INT_SAMPLER_1D = 0x00008DC9;
public const int INT_SAMPLER_2D = 0x00008DCA;
public const int INT_SAMPLER_3D = 0x00008DCB;
public const int INT_SAMPLER_CUBE = 0x00008DCC;
public const int INT_SAMPLER_1D_ARRAY = 0x00008DCE;
public const int INT_SAMPLER_2D_ARRAY = 0x00008DCF;
public const int UNSIGNED_INT_SAMPLER_1D = 0x00008DD1;
public const int UNSIGNED_INT_SAMPLER_2D = 0x00008DD2;
public const int UNSIGNED_INT_SAMPLER_3D = 0x00008DD3;
public const int UNSIGNED_INT_SAMPLER_CUBE = 0x00008DD4;
public const int UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x00008DD6;
public const int UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x00008DD7;
public const int QUERY_WAIT = 0x00008E13;
public const int QUERY_NO_WAIT = 0x00008E14;
public const int QUERY_BY_REGION_WAIT = 0x00008E15;
public const int QUERY_BY_REGION_NO_WAIT = 0x00008E16;
public const int BUFFER_ACCESS_FLAGS = 0x0000911F;
public const int BUFFER_MAP_LENGTH = 0x00009120;
public const int BUFFER_MAP_OFFSET = 0x00009121;
public const int DEPTH_COMPONENT32F = 0x00008CAC;
public const int DEPTH32F_STENCIL8 = 0x00008CAD;
public const int single_32_UNSIGNED_INT_24_8_REV = 0x00008DAD;
public const int INVALID_FRAMEBUFFER_OPERATION = 0x00000506;
public const int FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x00008210;
public const int FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x00008211;
public const int FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x00008212;
public const int FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x00008213;
public const int FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x00008214;
public const int FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x00008215;
public const int FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x00008216;
public const int FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x00008217;
public const int FRAMEBUFFER_DEFAULT = 0x00008218;
public const int FRAMEBUFFER_UNDEFINED = 0x00008219;
public const int DEPTH_STENCIL_ATTACHMENT = 0x0000821A;
public const int MAX_RENDERBUFFER_SIZE = 0x000084E8;
public const int DEPTH_STENCIL = 0x000084F9;
public const int UNSIGNED_INT_24_8 = 0x000084FA;
public const int DEPTH24_STENCIL8 = 0x000088F0;
public const int TEXTURE_STENCIL_SIZE = 0x000088F1;
public const int TEXTURE_RED_TYPE = 0x00008C10;
public const int TEXTURE_GREEN_TYPE = 0x00008C11;
public const int TEXTURE_BLUE_TYPE = 0x00008C12;
public const int TEXTURE_ALPHA_TYPE = 0x00008C13;
public const int TEXTURE_DEPTH_TYPE = 0x00008C16;
public const int UNSIGNED_NORMALIZED = 0x00008C17;
public const int FRAMEBUFFER_BINDING = 0x00008CA6;
public const int DRAW_FRAMEBUFFER_BINDING = 0x00008CA6;
public const int RENDERBUFFER_BINDING = 0x00008CA7;
public const int READ_FRAMEBUFFER = 0x00008CA8;
public const int DRAW_FRAMEBUFFER = 0x00008CA9;
public const int READ_FRAMEBUFFER_BINDING = 0x00008CAA;
public const int RENDERBUFFER_SAMPLES = 0x00008CAB;
public const int FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x00008CD0;
public const int FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x00008CD1;
public const int FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x00008CD2;
public const int FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x00008CD3;
public const int FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x00008CD4;
public const int FRAMEBUFFER_COMPLETE = 0x00008CD5;
public const int FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x00008CD6;
public const int FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x00008CD7;
public const int FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x00008CDB;
public const int FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x00008CDC;
public const int FRAMEBUFFER_UNSUPPORTED = 0x00008CDD;
public const int MAX_COLOR_ATTACHMENTS = 0x00008CDF;
public const int COLOR_ATTACHMENT0 = 0x00008CE0;
public const int COLOR_ATTACHMENT1 = 0x00008CE1;
public const int COLOR_ATTACHMENT2 = 0x00008CE2;
public const int COLOR_ATTACHMENT3 = 0x00008CE3;
public const int COLOR_ATTACHMENT4 = 0x00008CE4;
public const int COLOR_ATTACHMENT5 = 0x00008CE5;
public const int COLOR_ATTACHMENT6 = 0x00008CE6;
public const int COLOR_ATTACHMENT7 = 0x00008CE7;
public const int COLOR_ATTACHMENT8 = 0x00008CE8;
public const int COLOR_ATTACHMENT9 = 0x00008CE9;
public const int COLOR_ATTACHMENT10 = 0x00008CEA;
public const int COLOR_ATTACHMENT11 = 0x00008CEB;
public const int COLOR_ATTACHMENT12 = 0x00008CEC;
public const int COLOR_ATTACHMENT13 = 0x00008CED;
public const int COLOR_ATTACHMENT14 = 0x00008CEE;
public const int COLOR_ATTACHMENT15 = 0x00008CEF;
public const int COLOR_ATTACHMENT16 = 0x00008CF0;
public const int COLOR_ATTACHMENT17 = 0x00008CF1;
public const int COLOR_ATTACHMENT18 = 0x00008CF2;
public const int COLOR_ATTACHMENT19 = 0x00008CF3;
public const int COLOR_ATTACHMENT20 = 0x00008CF4;
public const int COLOR_ATTACHMENT21 = 0x00008CF5;
public const int COLOR_ATTACHMENT22 = 0x00008CF6;
public const int COLOR_ATTACHMENT23 = 0x00008CF7;
public const int COLOR_ATTACHMENT24 = 0x00008CF8;
public const int COLOR_ATTACHMENT25 = 0x00008CF9;
public const int COLOR_ATTACHMENT26 = 0x00008CFA;
public const int COLOR_ATTACHMENT27 = 0x00008CFB;
public const int COLOR_ATTACHMENT28 = 0x00008CFC;
public const int COLOR_ATTACHMENT29 = 0x00008CFD;
public const int COLOR_ATTACHMENT30 = 0x00008CFE;
public const int COLOR_ATTACHMENT31 = 0x00008CFF;
public const int DEPTH_ATTACHMENT = 0x00008D00;
public const int STENCIL_ATTACHMENT = 0x00008D20;
public const int FRAMEBUFFER = 0x00008D40;
public const int RENDERBUFFER = 0x00008D41;
public const int RENDERBUFFER_WIDTH = 0x00008D42;
public const int RENDERBUFFER_HEIGHT = 0x00008D43;
public const int RENDERBUFFER_INTERNAL_FORMAT = 0x00008D44;
public const int STENCIL_INDEX1 = 0x00008D46;
public const int STENCIL_INDEX4 = 0x00008D47;
public const int STENCIL_INDEX8 = 0x00008D48;
public const int STENCIL_INDEX16 = 0x00008D49;
public const int RENDERBUFFER_RED_SIZE = 0x00008D50;
public const int RENDERBUFFER_GREEN_SIZE = 0x00008D51;
public const int RENDERBUFFER_BLUE_SIZE = 0x00008D52;
public const int RENDERBUFFER_ALPHA_SIZE = 0x00008D53;
public const int RENDERBUFFER_DEPTH_SIZE = 0x00008D54;
public const int RENDERBUFFER_STENCIL_SIZE = 0x00008D55;
public const int FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x00008D56;
public const int MAX_SAMPLES = 0x00008D57;
public const int FRAMEBUFFER_SRGB = 0x00008DB9;
public const int HALF_single = 0x0000140B;
public const int MAP_READ_BIT = 0x00000001;
public const int MAP_WRITE_BIT = 0x00000002;
public const int MAP_INVALIDATE_RANGE_BIT = 0x00000004;
public const int MAP_INVALIDATE_BUFFER_BIT = 0x00000008;
public const int MAP_FLUSH_EXPLICIT_BIT = 0x00000010;
public const int MAP_UNSYNCHRONIZED_BIT = 0x00000020;
public const int COMPRESSED_RED_RGTC1 = 0x00008DBB;
public const int COMPRESSED_SIGNED_RED_RGTC1 = 0x00008DBC;
public const int COMPRESSED_RG_RGTC2 = 0x00008DBD;
public const int COMPRESSED_SIGNED_RG_RGTC2 = 0x00008DBE;
public const int RG = 0x00008227;
public const int RG_int = 0x00008228;
public const int R8 = 0x00008229;
public const int R16 = 0x0000822A;
public const int RG8 = 0x0000822B;
public const int RG16 = 0x0000822C;
public const int R16F = 0x0000822D;
public const int R32F = 0x0000822E;
public const int RG16F = 0x0000822F;
public const int RG32F = 0x00008230;
public const int R8I = 0x00008231;
public const int R8UI = 0x00008232;
public const int R16I = 0x00008233;
public const int R16UI = 0x00008234;
public const int R32I = 0x00008235;
public const int R32UI = 0x00008236;
public const int RG8I = 0x00008237;
public const int RG8UI = 0x00008238;
public const int RG16I = 0x00008239;
public const int RG16UI = 0x0000823A;
public const int RG32I = 0x0000823B;
public const int RG32UI = 0x0000823C;
public const int VERTEX_ARRAY_BINDING = 0x000085B5;
 // public static public const VERSION_3_2 1
 //
 // public static public const CONTEXT_CORE_PROFILE_BIT As int  = 0x000000000001
 // public static public const CONTEXT_COMPATIBILITY_PROFILE_BIT As int  = 0x000000000002
 // public static public const LINES_ADJACENCY As int  = 0x0000000A
 // public static public const LINE_STRIP_ADJACENCY As int  = 0x0000000B
 // public static public const TRIANGLES_ADJACENCY As int  = 0x0000000C
 // public static public const TRIANGLE_STRIP_ADJACENCY As int  = 0x0000000D
 // public static public const PROGRAM_POINT_SIZE As int  = 0x00008642
 // public static public const MAX_GEOMETRY_TEXTURE_IMAGE_UNITS As int  = 0x00008C29
 // public static public const FRAMEBUFFER_ATTACHMENT_LAYERED As int  = 0x00008DA7
 // public static public const FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS As int  = 0x00008DA8
 // public static public const GEOMETRY_SHADER As int  = 0x00008DD9
 // public static public const GEOMETRY_VERTICES_OUT As int  = 0x00008916
 // public static public const GEOMETRY_INPUT_TYPE As int  = 0x00008917
 // public static public const GEOMETRY_OUTPUT_TYPE As int  = 0x00008918
 // public static public const MAX_GEOMETRY_UNIFORM_COMPONENTS As int  = 0x00008DDF
 // public static public const MAX_GEOMETRY_OUTPUT_VERTICES As int  = 0x00008DE0
 // public static public const MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS As int  = 0x00008DE1
 // public static public const MAX_VERTEX_OUTPUT_COMPONENTS As int  = 0x00009122
 // public static public const MAX_GEOMETRY_INPUT_COMPONENTS As int  = 0x00009123
 // public static public const MAX_GEOMETRY_OUTPUT_COMPONENTS As int  = 0x00009124
 // public static public const MAX_FRAGMENT_INPUT_COMPONENTS As int  = 0x00009125
 // public static public const CONTEXT_PROFILE_MASK As int  = 0x00009126
 // public static public const DEPTH_CLAMP As int  = 0x0000864F
 // public static public const QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION As int  = 0x00008E4C
 // public static public const FIRST_VERTEX_CONVENTION As int  = 0x00008E4D
 // public static public const LAST_VERTEX_CONVENTION As int  = 0x00008E4E
 // public static public const PROVOKING_VERTEX As int  = 0x00008E4F
 // public static public const TEXTURE_CUBE_MAP_SEAMLESS As int  = 0x0000884F
 // public static public const MAX_SERVER_WAIT_TIMEOUT As int  = 0x00009111
 // public static public const OBJECT_TYPE As int  = 0x00009112
 // public static public const SYNC_CONDITION As int  = 0x00009113
 // public static public const SYNC_STATUS As int  = 0x00009114
 // public static public const SYNC_FLAGS As int  = 0x00009115
 // public static public const SYNC_FENCE As int  = 0x00009116
 // public static public const SYNC_GPU_COMMANDS_COMPLETE As int  = 0x00009117
 // public static public const UNSIGNALED As int  = 0x00009118
 // public static public const SIGNALED As int  = 0x00009119
 // public static public const ALREADY_SIGNALED As int  = 0x0000911A
 // public static public const TIMEOUT_EXPIRED As int  = 0x0000911B
 // public static public const CONDITION_SATISFIED As int  = 0x0000911C
 // public static public const WAIT_FAILED As int  = 0x0000911D
 // public static public const TIMEOUT_IGNORED As int = & HFFFFFFFFFFFFFFFFull
 // public static public const SYNC_FLUSH_COMMANDS_BIT As int  = 0x000000000001
 // public static public const SAMPLE_POSITION As int  = 0x00008E50
 // public static public const SAMPLE_MASK As int  = 0x00008E51
 // public static public const SAMPLE_MASK_VALUE As int  = 0x00008E52
 // public static public const MAX_SAMPLE_MASK_WORDS As int  = 0x00008E59
 // public static public const TEXTURE_2D_MULTISAMPLE As int  = 0x00009100
 // public static public const PROXY_TEXTURE_2D_MULTISAMPLE As int  = 0x00009101
 // public static public const TEXTURE_2D_MULTISAMPLE_ARRAY As int  = 0x00009102
 // public static public const PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY As int  = 0x00009103
 // public static public const TEXTURE_BINDING_2D_MULTISAMPLE As int  = 0x00009104
 // public static public const TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY As int  = 0x00009105
 // public static public const TEXTURE_SAMPLES As int  = 0x00009106
 // public static public const TEXTURE_FIXED_SAMPLE_LOCATIONS As int  = 0x00009107
 // public static public const SAMPLER_2D_MULTISAMPLE As int  = 0x00009108
 // public static public const INT_SAMPLER_2D_MULTISAMPLE As int  = 0x00009109
 // public static public const UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE As int  = 0x0000910A
 // public static public const SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910B
 // public static public const INT_SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910C
 // public static public const UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910D
 // public static public const MAX_COLOR_TEXTURE_SAMPLES As int  = 0x0000910E
 // public static public const MAX_DEPTH_TEXTURE_SAMPLES As int  = 0x0000910F
 // public static public const MAX_int_SAMPLES As int  = 0x00009110
 // public static public const VERSION_3_1 1
 // public static public const SAMPLER_2D_RECT As int  = 0x00008B63
 // public static public const SAMPLER_2D_RECT_SHADOW As int  = 0x00008B64
 // public static public const SAMPLER_BUFFER As int  = 0x00008DC2
 // public static public const INT_SAMPLER_2D_RECT As int  = 0x00008DCD
 // public static public const INT_SAMPLER_BUFFER As int  = 0x00008DD0
 // public static public const UNSIGNED_INT_SAMPLER_2D_RECT As int  = 0x00008DD5
 // public static public const UNSIGNED_INT_SAMPLER_BUFFER As int  = 0x00008DD8
 // public static public const TEXTURE_BUFFER As int  = 0x00008C2A
 // public static public const MAX_TEXTURE_BUFFER_SIZE As int  = 0x00008C2B
 // public static public const TEXTURE_BINDING_BUFFER As int  = 0x00008C2C
 // public static public const TEXTURE_BUFFER_DATA_STORE_BINDING As int  = 0x00008C2D
 // public static public const TEXTURE_RECTANGLE As int  = 0x000084F5
 // public static public const TEXTURE_BINDING_RECTANGLE As int  = 0x000084F6
 // public static public const PROXY_TEXTURE_RECTANGLE As int  = 0x000084F7
 // public static public const MAX_RECTANGLE_TEXTURE_SIZE As int  = 0x000084F8
 // public static public const R8_SNORM As int  = 0x00008F94
 // public static public const RG8_SNORM As int  = 0x00008F95
 // public static public const RGB8_SNORM As int  = 0x00008F96
 // public static public const RGBA8_SNORM As int  = 0x00008F97
 // public static public const R16_SNORM As int  = 0x00008F98
 // public static public const RG16_SNORM As int  = 0x00008F99
 // public static public const RGB16_SNORM As int  = 0x00008F9A
 // public static public const RGBA16_SNORM As int  = 0x00008F9B
 // public static public const SIGNED_NORMALIZED As int  = 0x00008F9C
 // public static public const PRIMITIVE_RESTART As int  = 0x00008F9D
 // public static public const PRIMITIVE_RESTART_INDEX As int  = 0x00008F9E
 // public static public const COPY_READ_BUFFER As int  = 0x00008F36
 // public static public const COPY_WRITE_BUFFER As int  = 0x00008F37
 // public static public const UNIFORM_BUFFER As int  = 0x00008A11
 // public static public const UNIFORM_BUFFER_BINDING As int  = 0x00008A28
 // public static public const UNIFORM_BUFFER_START As int  = 0x00008A29
 // public static public const UNIFORM_BUFFER_SIZE As int  = 0x00008A2A
 // public static public const MAX_VERTEX_UNIFORM_BLOCKS As int  = 0x00008A2B
 // public static public const MAX_GEOMETRY_UNIFORM_BLOCKS As int  = 0x00008A2C
 // public static public const MAX_FRAGMENT_UNIFORM_BLOCKS As int  = 0x00008A2D
 // public static public const MAX_COMBINED_UNIFORM_BLOCKS As int  = 0x00008A2E
 // public static public const MAX_UNIFORM_BUFFER_BINDINGS As int  = 0x00008A2F
 // public static public const MAX_UNIFORM_BLOCK_SIZE As int  = 0x00008A30
 // public static public const MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS As int  = 0x00008A31
 // public static public const MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS As int  = 0x00008A32
 // public static public const MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS As int  = 0x00008A33
 // public static public const UNIFORM_BUFFER_OFFSET_ALIGNMENT As int  = 0x00008A34
 // public static public const ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH As int  = 0x00008A35
 // public static public const ACTIVE_UNIFORM_BLOCKS As int  = 0x00008A36
 // public static public const UNIFORM_TYPE As int  = 0x00008A37
 // public static public const UNIFORM_SIZE As int  = 0x00008A38
 // public static public const UNIFORM_NAME_LENGTH As int  = 0x00008A39
 // public static public const UNIFORM_BLOCK_INDEX As int  = 0x00008A3A
 // public static public const UNIFORM_OFFSET As int  = 0x00008A3B
 // public static public const UNIFORM_ARRAY_STRIDE As int  = 0x00008A3C
 // public static public const UNIFORM_MATRIX_STRIDE As int  = 0x00008A3D
 // public static public const UNIFORM_IS_ROW_MAJOR As int  = 0x00008A3E
 // public static public const UNIFORM_BLOCK_BINDING As int  = 0x00008A3F
 // public static public const UNIFORM_BLOCK_DATA_SIZE As int  = 0x00008A40
 // public static public const UNIFORM_BLOCK_NAME_LENGTH As int  = 0x00008A41
 // public static public const UNIFORM_BLOCK_ACTIVE_UNIFORMS As int  = 0x00008A42
 // public static public const UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES As int  = 0x00008A43
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER As int  = 0x00008A44
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER As int  = 0x00008A45
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER As int  = 0x00008A46
 // public static public const INVALID_INDEX As int = & HFFFFFFFFu
 // public static public const VERSION_4_3 1
 // public static public const NUM_SHADING_LANGUAGE_VERSIONS As int  = 0x000082E9
 // public static public const VERTEX_ATTRIB_ARRAY_LONG As int  = 0x0000874E
 // public static public const COMPRESSED_RGB8_ETC2 As int  = 0x00009274
 // public static public const COMPRESSED_SRGB8_ETC2 As int  = 0x00009275
 // public static public const COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 As int  = 0x00009276
 // public static public const COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 As int  = 0x00009277
 // public static public const COMPRESSED_RGBA8_ETC2_EAC As int  = 0x00009278
 // public static public const COMPRESSED_SRGB8_ALPHA8_ETC2_EAC As int  = 0x00009279
 // public static public const COMPRESSED_R11_EAC As int  = 0x00009270
 // public static public const COMPRESSED_SIGNED_R11_EAC As int  = 0x00009271
 // public static public const COMPRESSED_RG11_EAC As int  = 0x00009272
 // public static public const COMPRESSED_SIGNED_RG11_EAC As int  = 0x00009273
 // public static public const PRIMITIVE_RESTART_FIXED_INDEX As int  = 0x00008D69
 // public static public const ANY_SAMPLES_PASSED_CONSERVATIVE As int  = 0x00008D6A
 // public static public const MAX_ELEMENT_INDEX As int  = 0x00008D6B
 // public static public const COMPUTE_SHADER As int  = 0x000091B9
 // public static public const MAX_COMPUTE_UNIFORM_BLOCKS As int  = 0x000091BB
 // public static public const MAX_COMPUTE_TEXTURE_IMAGE_UNITS As int  = 0x000091BC
 // public static public const MAX_COMPUTE_IMAGE_UNIFORMS As int  = 0x000091BD
 // public static public const MAX_COMPUTE_SHARED_MEMORY_SIZE As int  = 0x00008262
 // public static public const MAX_COMPUTE_UNIFORM_COMPONENTS As int  = 0x00008263
 // public static public const MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS As int  = 0x00008264
 // public static public const MAX_COMPUTE_ATOMIC_COUNTERS As int  = 0x00008265
 // public static public const MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS As int  = 0x00008266
 // public static public const MAX_COMPUTE_WORK_GROUP_INVOCATIONS As int  = 0x000090EB
 // public static public const MAX_COMPUTE_WORK_GROUP_COUNT As int  = 0x000091BE
 // public static public const MAX_COMPUTE_WORK_GROUP_SIZE As int  = 0x000091BF
 // public static public const COMPUTE_WORK_GROUP_SIZE As int  = 0x00008267
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER As int  = 0x000090EC
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER As int  = 0x000090ED
 // public static public const DISPATCH_INDIRECT_BUFFER As int  = 0x000090EE
 // public static public const DISPATCH_INDIRECT_BUFFER_BINDING As int  = 0x000090EF
 // public static public const COMPUTE_SHADER_BIT As int  = 0x000000000020
 // public static public const Console.WriteLine(_OUTPUT_SYNCHRONOUS As int  = 0x00008242
 // public static public const Console.WriteLine(_NEXT_LOGGED_MESSAGE_LENGTH As int  = 0x00008243
 // public static public const Console.WriteLine(_CALLBACK_FUNCTION As int  = 0x00008244
 // public static public const Console.WriteLine(_CALLBACK_USER_PARAM As int  = 0x00008245
 // public static public const Console.WriteLine(_SOURCE_API As int  = 0x00008246
 // public static public const Console.WriteLine(_SOURCE_WINDOW_SYSTEM As int  = 0x00008247
 // public static public const Console.WriteLine(_SOURCE_SHADER_COMPILER As int  = 0x00008248
 // public static public const Console.WriteLine(_SOURCE_THIRD_PARTY As int  = 0x00008249
 // public static public const Console.WriteLine(_SOURCE_APPLICATION As int  = 0x0000824A
 // public static public const Console.WriteLine(_SOURCE_OTHER As int  = 0x0000824B
 // public static public const Console.WriteLine(_TYPE_ERROR As int  = 0x0000824C
 // public static public const Console.WriteLine(_TYPE_DEPRECATED_BEHAVIOR As int  = 0x0000824D
 // public static public const Console.WriteLine(_TYPE_UNDEFINED_BEHAVIOR As int  = 0x0000824E
 // public static public const Console.WriteLine(_TYPE_PORTABILITY As int  = 0x0000824F
 // public static public const Console.WriteLine(_TYPE_PERFORMANCE As int  = 0x00008250
 // public static public const Console.WriteLine(_TYPE_OTHER As int  = 0x00008251
 // public static public const MAX_Console.WriteLine(_MESSAGE_LENGTH As int  = 0x00009143
 // public static public const MAX_Console.WriteLine(_LOGGED_MESSAGES As int  = 0x00009144
 // public static public const Console.WriteLine(_LOGGED_MESSAGES As int  = 0x00009145
 // public static public const Console.WriteLine(_SEVERITY_HIGH As int  = 0x00009146
 // public static public const Console.WriteLine(_SEVERITY_MEDIUM As int  = 0x00009147
 // public static public const Console.WriteLine(_SEVERITY_LOW As int  = 0x00009148
 // public static public const Console.WriteLine(_TYPE_MARKER As int  = 0x00008268
 // public static public const Console.WriteLine(_TYPE_PUSH_GROUP As int  = 0x00008269
 // public static public const Console.WriteLine(_TYPE_POP_GROUP As int  = 0x0000826A
 // public static public const Console.WriteLine(_SEVERITY_NOTIFICATION As int  = 0x0000826B
 // public static public const MAX_Console.WriteLine(_GROUP_STACK_DEPTH As int  = 0x0000826C
 // public static public const Console.WriteLine(_GROUP_STACK_DEPTH As int  = 0x0000826D
 // public static public const BUFFER As int  = 0x000082E0
 // public static public const SHADER As int  = 0x000082E1
 // public static public const PROGRAM As int  = 0x000082E2
 // public static public const QUERY As int  = 0x000082E3
 // public static public const PROGRAM_PIPELINE As int  = 0x000082E4
 // public static public const SAMPLER As int  = 0x000082E6
 // public static public const MAX_LABEL_LENGTH As int  = 0x000082E8
 // public static public const Console.WriteLine(_OUTPUT As int  = 0x000092E0
 // public static public const CONTEXT_FLAG_Console.WriteLine(_BIT As int  = 0x000000000002
 // public static public const MAX_UNIFORM_LOCATIONS As int  = 0x0000826E
 // public static public const FRAMEBUFFER_DEFAULT_WIDTH As int  = 0x00009310
 // public static public const FRAMEBUFFER_DEFAULT_HEIGHT As int  = 0x00009311
 // public static public const FRAMEBUFFER_DEFAULT_LAYERS As int  = 0x00009312
 // public static public const FRAMEBUFFER_DEFAULT_SAMPLES As int  = 0x00009313
 // public static public const FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS As int  = 0x00009314
 // public static public const MAX_FRAMEBUFFER_WIDTH As int  = 0x00009315
 // public static public const MAX_FRAMEBUFFER_HEIGHT As int  = 0x00009316
 // public static public const MAX_FRAMEBUFFER_LAYERS As int  = 0x00009317
 // public static public const MAX_FRAMEBUFFER_SAMPLES As int  = 0x00009318
 // public static public const INTERNALFORMAT_SUPPORTED As int  = 0x0000826F
 // public static public const INTERNALFORMAT_PREFERRED As int  = 0x00008270
 // public static public const INTERNALFORMAT_RED_SIZE As int  = 0x00008271
 // public static public const INTERNALFORMAT_GREEN_SIZE As int  = 0x00008272
 // public static public const INTERNALFORMAT_BLUE_SIZE As int  = 0x00008273
 // public static public const INTERNALFORMAT_ALPHA_SIZE As int  = 0x00008274
 // public static public const INTERNALFORMAT_DEPTH_SIZE As int  = 0x00008275
 // public static public const INTERNALFORMAT_STENCIL_SIZE As int  = 0x00008276
 // public static public const INTERNALFORMAT_SHARED_SIZE As int  = 0x00008277
 // public static public const INTERNALFORMAT_RED_TYPE As int  = 0x00008278
 // public static public const INTERNALFORMAT_GREEN_TYPE As int  = 0x00008279
 // public static public const INTERNALFORMAT_BLUE_TYPE As int  = 0x0000827A
 // public static public const INTERNALFORMAT_ALPHA_TYPE As int  = 0x0000827B
 // public static public const INTERNALFORMAT_DEPTH_TYPE As int  = 0x0000827C
 // public static public const INTERNALFORMAT_STENCIL_TYPE As int  = 0x0000827D
 // public static public const MAX_WIDTH As int  = 0x0000827E
 // public static public const MAX_HEIGHT As int  = 0x0000827F
 // public static public const MAX_DEPTH As int  = 0x00008280
 // public static public const MAX_LAYERS As int  = 0x00008281
 // public static public const MAX_COMBINED_DIMENSIONS As int  = 0x00008282
 // public static public const COLOR_COMPONENTS As int  = 0x00008283
 // public static public const DEPTH_COMPONENTS As int  = 0x00008284
 // public static public const STENCIL_COMPONENTS As int  = 0x00008285
 // public static public const COLOR_RENDERABLE As int  = 0x00008286
 // public static public const DEPTH_RENDERABLE As int  = 0x00008287
 // public static public const STENCIL_RENDERABLE As int  = 0x00008288
 // public static public const FRAMEBUFFER_RENDERABLE As int  = 0x00008289
 // public static public const FRAMEBUFFER_RENDERABLE_LAYERED As int  = 0x0000828A
 // public static public const FRAMEBUFFER_BLEND As int  = 0x0000828B
 // public static public const READ_PIXELS As int  = 0x0000828C
 // public static public const READ_PIXELS_FORMAT As int  = 0x0000828D
 // public static public const READ_PIXELS_TYPE As int  = 0x0000828E
 // public static public const TEXTURE_IMAGE_FORMAT As int  = 0x0000828F
 // public static public const TEXTURE_IMAGE_TYPE As int  = 0x00008290
 // public static public const GET_TEXTURE_IMAGE_FORMAT As int  = 0x00008291
 // public static public const GET_TEXTURE_IMAGE_TYPE As int  = 0x00008292
 // public static public const MIPMAP As int  = 0x00008293
 // public static public const MANUAL_GENERATE_MIPMAP As int  = 0x00008294
 // public static public const AUTO_GENERATE_MIPMAP As int  = 0x00008295
 // public static public const COLOR_ENCODING As int  = 0x00008296
 // public static public const SRGB_READ As int  = 0x00008297
 // public static public const SRGB_WRITE As int  = 0x00008298
 // public static public const FILTER As int  = 0x0000829A
 // public static public const VERTEX_TEXTURE As int  = 0x0000829B
 // public static public const TESS_CONTROL_TEXTURE As int  = 0x0000829C
 // public static public const TESS_EVALUATION_TEXTURE As int  = 0x0000829D
 // public static public const GEOMETRY_TEXTURE As int  = 0x0000829E
 // public static public const FRAGMENT_TEXTURE As int  = 0x0000829F
 // public static public const COMPUTE_TEXTURE As int  = 0x000082A0
 // public static public const TEXTURE_SHADOW As int  = 0x000082A1
 // public static public const TEXTURE_GATHER As int  = 0x000082A2
 // public static public const TEXTURE_GATHER_SHADOW As int  = 0x000082A3
 // public static public const SHADER_IMAGE_LOAD As int  = 0x000082A4
 // public static public const SHADER_IMAGE_STORE As int  = 0x000082A5
 // public static public const SHADER_IMAGE_ATOMIC As int  = 0x000082A6
 // public static public const IMAGE_TEXEL_SIZE As int  = 0x000082A7
 // public static public const IMAGE_COMPATIBILITY_CLASS As int  = 0x000082A8
 // public static public const IMAGE_PIXEL_FORMAT As int  = 0x000082A9
 // public static public const IMAGE_PIXEL_TYPE As int  = 0x000082AA
 // public static public const SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST As int  = 0x000082AC
 // public static public const SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST As int  = 0x000082AD
 // public static public const SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE As int  = 0x000082AE
 // public static public const SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE As int  = 0x000082AF
 // public static public const TEXTURE_COMPRESSED_BLOCK_WIDTH As int  = 0x000082B1
 // public static public const TEXTURE_COMPRESSED_BLOCK_HEIGHT As int  = 0x000082B2
 // public static public const TEXTURE_COMPRESSED_BLOCK_SIZE As int  = 0x000082B3
 // public static public const CLEAR_BUFFER As int  = 0x000082B4
 // public static public const TEXTURE_VIEW As int  = 0x000082B5
 // public static public const VIEW_COMPATIBILITY_CLASS As int  = 0x000082B6
 // public static public const FULL_SUPPORT As int  = 0x000082B7
 // public static public const CAVEAT_SUPPORT As int  = 0x000082B8
 // public static public const IMAGE_CLASS_4_X_32 As int  = 0x000082B9
 // public static public const IMAGE_CLASS_2_X_32 As int  = 0x000082BA
 // public static public const IMAGE_CLASS_1_X_32 As int  = 0x000082BB
 // public static public const IMAGE_CLASS_4_X_16 As int  = 0x000082BC
 // public static public const IMAGE_CLASS_2_X_16 As int  = 0x000082BD
 // public static public const IMAGE_CLASS_1_X_16 As int  = 0x000082BE
 // public static public const IMAGE_CLASS_4_X_8 As int  = 0x000082BF
 // public static public const IMAGE_CLASS_2_X_8 As int  = 0x000082C0
 // public static public const IMAGE_CLASS_1_X_8 As int  = 0x000082C1
 // public static public const IMAGE_CLASS_11_11_10 As int  = 0x000082C2
 // public static public const IMAGE_CLASS_10_10_10_2 As int  = 0x000082C3
 // public static public const VIEW_CLASS_128_BITS As int  = 0x000082C4
 // public static public const VIEW_CLASS_96_BITS As int  = 0x000082C5
 // public static public const VIEW_CLASS_64_BITS As int  = 0x000082C6
 // public static public const VIEW_CLASS_48_BITS As int  = 0x000082C7
 // public static public const VIEW_CLASS_32_BITS As int  = 0x000082C8
 // public static public const VIEW_CLASS_24_BITS As int  = 0x000082C9
 // public static public const VIEW_CLASS_16_BITS As int  = 0x000082CA
 // public static public const VIEW_CLASS_8_BITS As int  = 0x000082CB
 // public static public const VIEW_CLASS_S3TC_DXT1_RGB As int  = 0x000082CC
 // public static public const VIEW_CLASS_S3TC_DXT1_RGBA As int  = 0x000082CD
 // public static public const VIEW_CLASS_S3TC_DXT3_RGBA As int  = 0x000082CE
 // public static public const VIEW_CLASS_S3TC_DXT5_RGBA As int  = 0x000082CF
 // public static public const VIEW_CLASS_RGTC1_RED As int  = 0x000082D0
 // public static public const VIEW_CLASS_RGTC2_RG As int  = 0x000082D1
 // public static public const VIEW_CLASS_BPTC_UNORM As int  = 0x000082D2
 // public static public const VIEW_CLASS_BPTC_single As int  = 0x000082D3
 // public static public const UNIFORM As int  = 0x000092E1
 // public static public const UNIFORM_BLOCK As int  = 0x000092E2
 // public static public const PROGRAM_INPUT As int  = 0x000092E3
 // public static public const PROGRAM_OUTPUT As int  = 0x000092E4
 // public static public const BUFFER_VARIABLE As int  = 0x000092E5
 // public static public const SHADER_STORAGE_BLOCK As int  = 0x000092E6
 // public static public const VERTEX_SUBROUTINE As int  = 0x000092E8
 // public static public const TESS_CONTROL_SUBROUTINE As int  = 0x000092E9
 // public static public const TESS_EVALUATION_SUBROUTINE As int  = 0x000092EA
 // public static public const GEOMETRY_SUBROUTINE As int  = 0x000092EB
 // public static public const FRAGMENT_SUBROUTINE As int  = 0x000092EC
 // public static public const COMPUTE_SUBROUTINE As int  = 0x000092ED
 // public static public const VERTEX_SUBROUTINE_UNIFORM As int  = 0x000092EE
 // public static public const TESS_CONTROL_SUBROUTINE_UNIFORM As int  = 0x000092EF
 // public static public const TESS_EVALUATION_SUBROUTINE_UNIFORM As int  = 0x000092F0
 // public static public const GEOMETRY_SUBROUTINE_UNIFORM As int  = 0x000092F1
 // public static public const FRAGMENT_SUBROUTINE_UNIFORM As int  = 0x000092F2
 // public static public const COMPUTE_SUBROUTINE_UNIFORM As int  = 0x000092F3
 // public static public const TRANSFORM_FEEDBACK_VARYING As int  = 0x000092F4
 // public static public const ACTIVE_RESOURCES As int  = 0x000092F5
 // public static public const MAX_NAME_LENGTH As int  = 0x000092F6
 // public static public const MAX_NUM_ACTIVE_VARIABLES As int  = 0x000092F7
 // public static public const MAX_NUM_COMPATIBLE_SUBROUTINES As int  = 0x000092F8
 // public static public const NAME_LENGTH As int  = 0x000092F9
 // public static public const TYPE As int  = 0x000092FA
 // public static public const ARRAY_SIZE As int  = 0x000092FB
 // public static public const OFFSET As int  = 0x000092FC
 // public static public const BLOCK_INDEX As int  = 0x000092FD
 // public static public const ARRAY_STRIDE As int  = 0x000092FE
 // public static public const MATRIX_STRIDE As int  = 0x000092FF
 // public static public const IS_ROW_MAJOR As int  = 0x00009300
 // public static public const ATOMIC_COUNTER_BUFFER_INDEX As int  = 0x00009301
 // public static public const BUFFER_BINDING As int  = 0x00009302
 // public static public const BUFFER_DATA_SIZE As int  = 0x00009303
 // public static public const NUM_ACTIVE_VARIABLES As int  = 0x00009304
 // public static public const ACTIVE_VARIABLES As int  = 0x00009305
 // public static public const REFERENCED_BY_VERTEX_SHADER As int  = 0x00009306
 // public static public const REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x00009307
 // public static public const REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x00009308
 // public static public const REFERENCED_BY_GEOMETRY_SHADER As int  = 0x00009309
 // public static public const REFERENCED_BY_FRAGMENT_SHADER As int  = 0x0000930A
 // public static public const REFERENCED_BY_COMPUTE_SHADER As int  = 0x0000930B
 // public static public const TOP_LEVEL_ARRAY_SIZE As int  = 0x0000930C
 // public static public const TOP_LEVEL_ARRAY_STRIDE As int  = 0x0000930D
 // public static public const LOCATION As int  = 0x0000930E
 // public static public const LOCATION_INDEX As int  = 0x0000930F
 // public static public const IS_PER_PATCH As int  = 0x000092E7
 // public static public const SHADER_STORAGE_BUFFER As int  = 0x000090D2
 // public static public const SHADER_STORAGE_BUFFER_BINDING As int  = 0x000090D3
 // public static public const SHADER_STORAGE_BUFFER_START As int  = 0x000090D4
 // public static public const SHADER_STORAGE_BUFFER_SIZE As int  = 0x000090D5
 // public static public const MAX_VERTEX_SHADER_STORAGE_BLOCKS As int  = 0x000090D6
 // public static public const MAX_GEOMETRY_SHADER_STORAGE_BLOCKS As int  = 0x000090D7
 // public static public const MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS As int  = 0x000090D8
 // public static public const MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS As int  = 0x000090D9
 // public static public const MAX_FRAGMENT_SHADER_STORAGE_BLOCKS As int  = 0x000090DA
 // public static public const MAX_COMPUTE_SHADER_STORAGE_BLOCKS As int  = 0x000090DB
 // public static public const MAX_COMBINED_SHADER_STORAGE_BLOCKS As int  = 0x000090DC
 // public static public const MAX_SHADER_STORAGE_BUFFER_BINDINGS As int  = 0x000090DD
 // public static public const MAX_SHADER_STORAGE_BLOCK_SIZE As int  = 0x000090DE
 // public static public const SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT As int  = 0x000090DF
 // public static public const SHADER_STORAGE_BARRIER_BIT As int  = 0x000000002000
 // public static public const MAX_COMBINED_SHADER_OUTPUT_RESOURCES As int  = 0x00008F39
 // public static public const DEPTH_STENCIL_TEXTURE_MODE As int  = 0x000090EA
 // public static public const TEXTURE_BUFFER_OFFSET As int  = 0x0000919D
 // public static public const TEXTURE_BUFFER_SIZE As int  = 0x0000919E
 // public static public const TEXTURE_BUFFER_OFFSET_ALIGNMENT As int  = 0x0000919F
 // public static public const TEXTURE_VIEW_MIN_LEVEL As int  = 0x000082DB
 // public static public const TEXTURE_VIEW_NUM_LEVELS As int  = 0x000082DC
 // public static public const TEXTURE_VIEW_MIN_LAYER As int  = 0x000082DD
 // public static public const TEXTURE_VIEW_NUM_LAYERS As int  = 0x000082DE
 // public static public const TEXTURE_IMMUTABLE_LEVELS As int  = 0x000082DF
 // public static public const VERTEX_ATTRIB_BINDING As int  = 0x000082D4
 // public static public const VERTEX_ATTRIB_RELATIVE_OFFSET As int  = 0x000082D5
 // public static public const VERTEX_BINDING_DIVISOR As int  = 0x000082D6
 // public static public const VERTEX_BINDING_OFFSET As int  = 0x000082D7
 // public static public const VERTEX_BINDING_STRIDE As int  = 0x000082D8
 // public static public const MAX_VERTEX_ATTRIB_RELATIVE_OFFSET As int  = 0x000082D9
 // public static public const MAX_VERTEX_ATTRIB_BINDINGS As int  = 0x000082DA
 // public static public const VERTEX_BINDING_BUFFER As int  = 0x00008F4F
 // public static public const VERSION_4_2 1
 // public static public const COPY_READ_BUFFER_BINDING As int  = 0x00008F36
 // public static public const COPY_WRITE_BUFFER_BINDING As int  = 0x00008F37
 // public static public const TRANSFORM_FEEDBACK_ACTIVE As int  = 0x00008E24
 // public static public const TRANSFORM_FEEDBACK_PAUSED As int  = 0x00008E23
 // public static public const UNPACK_COMPRESSED_BLOCK_WIDTH As int  = 0x00009127
 // public static public const UNPACK_COMPRESSED_BLOCK_HEIGHT As int  = 0x00009128
 // public static public const UNPACK_COMPRESSED_BLOCK_DEPTH As int  = 0x00009129
 // public static public const UNPACK_COMPRESSED_BLOCK_SIZE As int  = 0x0000912A
 // public static public const PACK_COMPRESSED_BLOCK_WIDTH As int  = 0x0000912B
 // public static public const PACK_COMPRESSED_BLOCK_HEIGHT As int  = 0x0000912C
 // public static public const PACK_COMPRESSED_BLOCK_DEPTH As int  = 0x0000912D
 // public static public const PACK_COMPRESSED_BLOCK_SIZE As int  = 0x0000912E
 // public static public const NUM_SAMPLE_COUNTS As int  = 0x00009380
 // public static public const MIN_MAP_BUFFER_ALIGNMENT As int  = 0x000090BC
 // public static public const ATOMIC_COUNTER_BUFFER As int  = 0x000092C0
 // public static public const ATOMIC_COUNTER_BUFFER_BINDING As int  = 0x000092C1
 // public static public const ATOMIC_COUNTER_BUFFER_START As int  = 0x000092C2
 // public static public const ATOMIC_COUNTER_BUFFER_SIZE As int  = 0x000092C3
 // public static public const ATOMIC_COUNTER_BUFFER_DATA_SIZE As int  = 0x000092C4
 // public static public const ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS As int  = 0x000092C5
 // public static public const ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES As int  = 0x000092C6
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER As int  = 0x000092C7
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x000092C8
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x000092C9
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER As int  = 0x000092CA
 // public static public const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER As int  = 0x000092CB
 // public static public const MAX_VERTEX_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CC
 // public static public const MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CD
 // public static public const MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CE
 // public static public const MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CF
 // public static public const MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D0
 // public static public const MAX_COMBINED_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D1
 // public static public const MAX_VERTEX_ATOMIC_COUNTERS As int  = 0x000092D2
 // public static public const MAX_TESS_CONTROL_ATOMIC_COUNTERS As int  = 0x000092D3
 // public static public const MAX_TESS_EVALUATION_ATOMIC_COUNTERS As int  = 0x000092D4
 // public static public const MAX_GEOMETRY_ATOMIC_COUNTERS As int  = 0x000092D5
 // public static public const MAX_FRAGMENT_ATOMIC_COUNTERS As int  = 0x000092D6
 // public static public const MAX_COMBINED_ATOMIC_COUNTERS As int  = 0x000092D7
 // public static public const MAX_ATOMIC_COUNTER_BUFFER_SIZE As int  = 0x000092D8
 // public static public const MAX_ATOMIC_COUNTER_BUFFER_BINDINGS As int  = 0x000092DC
 // public static public const ACTIVE_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D9
 // public static public const UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX As int  = 0x000092DA
 // public static public const UNSIGNED_INT_ATOMIC_COUNTER As int  = 0x000092DB
 // public static public const VERTEX_ATTRIB_ARRAY_BARRIER_BIT As int  = 0x000000000001
 // public static public const ELEMENT_ARRAY_BARRIER_BIT As int  = 0x000000000002
 // public static public const UNIFORM_BARRIER_BIT As int  = 0x000000000004
 // public static public const TEXTURE_FETCH_BARRIER_BIT As int  = 0x000000000008
 // public static public const SHADER_IMAGE_ACCESS_BARRIER_BIT As int  = 0x000000000020
 // public static public const COMMAND_BARRIER_BIT As int  = 0x000000000040
 // public static public const PIXEL_BUFFER_BARRIER_BIT As int  = 0x000000000080
 // public static public const TEXTURE_UPDATE_BARRIER_BIT As int  = 0x000000000100
 // public static public const BUFFER_UPDATE_BARRIER_BIT As int  = 0x000000000200
 // public static public const FRAMEBUFFER_BARRIER_BIT As int  = 0x000000000400
 // public static public const TRANSFORM_FEEDBACK_BARRIER_BIT As int  = 0x000000000800
 // public static public const ATOMIC_COUNTER_BARRIER_BIT As int  = 0x000000001000
 // public static public const ALL_BARRIER_BITS As int  = 0x0000FFFFFFFF
 // public static public const MAX_IMAGE_UNITS As int  = 0x00008F38
 // public static public const MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS As int  = 0x00008F39
 // public static public const IMAGE_BINDING_NAME As int  = 0x00008F3A
 // public static public const IMAGE_BINDING_LEVEL As int  = 0x00008F3B
 // public static public const IMAGE_BINDING_LAYERED As int  = 0x00008F3C
 // public static public const IMAGE_BINDING_LAYER As int  = 0x00008F3D
 // public static public const IMAGE_BINDING_ACCESS As int  = 0x00008F3E
 // public static public const IMAGE_1D As int  = 0x0000904C
 // public static public const IMAGE_2D As int  = 0x0000904D
 // public static public const IMAGE_3D As int  = 0x0000904E
 // public static public const IMAGE_2D_RECT As int  = 0x0000904F
 // public static public const IMAGE_CUBE As int  = 0x00009050
 // public static public const IMAGE_BUFFER As int  = 0x00009051
 // public static public const IMAGE_1D_ARRAY As int  = 0x00009052
 // public static public const IMAGE_2D_ARRAY As int  = 0x00009053
 // public static public const IMAGE_CUBE_MAP_ARRAY As int  = 0x00009054
 // public static public const IMAGE_2D_MULTISAMPLE As int  = 0x00009055
 // public static public const IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x00009056
 // public static public const INT_IMAGE_1D As int  = 0x00009057
 // public static public const INT_IMAGE_2D As int  = 0x00009058
 // public static public const INT_IMAGE_3D As int  = 0x00009059
 // public static public const INT_IMAGE_2D_RECT As int  = 0x0000905A
 // public static public const INT_IMAGE_CUBE As int  = 0x0000905B
 // public static public const INT_IMAGE_BUFFER As int  = 0x0000905C
 // public static public const INT_IMAGE_1D_ARRAY As int  = 0x0000905D
 // public static public const INT_IMAGE_2D_ARRAY As int  = 0x0000905E
 // public static public const INT_IMAGE_CUBE_MAP_ARRAY As int  = 0x0000905F
 // public static public const INT_IMAGE_2D_MULTISAMPLE As int  = 0x00009060
 // public static public const INT_IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x00009061
 // public static public const UNSIGNED_INT_IMAGE_1D As int  = 0x00009062
 // public static public const UNSIGNED_INT_IMAGE_2D As int  = 0x00009063
 // public static public const UNSIGNED_INT_IMAGE_3D As int  = 0x00009064
 // public static public const UNSIGNED_INT_IMAGE_2D_RECT As int  = 0x00009065
 // public static public const UNSIGNED_INT_IMAGE_CUBE As int  = 0x00009066
 // public static public const UNSIGNED_INT_IMAGE_BUFFER As int  = 0x00009067
 // public static public const UNSIGNED_INT_IMAGE_1D_ARRAY As int  = 0x00009068
 // public static public const UNSIGNED_INT_IMAGE_2D_ARRAY As int  = 0x00009069
 // public static public const UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY As int  = 0x0000906A
 // public static public const UNSIGNED_INT_IMAGE_2D_MULTISAMPLE As int  = 0x0000906B
 // public static public const UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x0000906C
 // public static public const MAX_IMAGE_SAMPLES As int  = 0x0000906D
 // public static public const IMAGE_BINDING_FORMAT As int  = 0x0000906E
 // public static public const IMAGE_FORMAT_COMPATIBILITY_TYPE As int  = 0x000090C7
 // public static public const IMAGE_FORMAT_COMPATIBILITY_BY_SIZE As int  = 0x000090C8
 // public static public const IMAGE_FORMAT_COMPATIBILITY_BY_CLASS As int  = 0x000090C9
 // public static public const MAX_VERTEX_IMAGE_UNIFORMS As int  = 0x000090CA
 // public static public const MAX_TESS_CONTROL_IMAGE_UNIFORMS As int  = 0x000090CB
 // public static public const MAX_TESS_EVALUATION_IMAGE_UNIFORMS As int  = 0x000090CC
 // public static public const MAX_GEOMETRY_IMAGE_UNIFORMS As int  = 0x000090CD
 // public static public const MAX_FRAGMENT_IMAGE_UNIFORMS As int  = 0x000090CE
 // public static public const MAX_COMBINED_IMAGE_UNIFORMS As int  = 0x000090CF
 // public static public const COMPRESSED_RGBA_BPTC_UNORM As int  = 0x00008E8C
 // public static public const COMPRESSED_SRGB_ALPHA_BPTC_UNORM As int  = 0x00008E8D
 // public static public const COMPRESSED_RGB_BPTC_SIGNED_single As int  = 0x00008E8E
 // public static public const COMPRESSED_RGB_BPTC_UNSIGNED_single As int  = 0x00008E8F
 // public static public const TEXTURE_IMMUTABLE_FORMAT As int  = 0x0000912F
 // public static public const VERSION_4_0 1
 // public static public const SAMPLE_SHADING As int  = 0x00008C36
 // public static public const MIN_SAMPLE_SHADING_VALUE As int  = 0x00008C37
 // public static public const MIN_PROGRAM_TEXTURE_GATHER_OFFSET As int  = 0x00008E5E
 // public static public const MAX_PROGRAM_TEXTURE_GATHER_OFFSET As int  = 0x00008E5F
 // public static public const TEXTURE_CUBE_MAP_ARRAY As int  = 0x00009009
 // public static public const TEXTURE_BINDING_CUBE_MAP_ARRAY As int  = 0x0000900A
 // public static public const PROXY_TEXTURE_CUBE_MAP_ARRAY As int  = 0x0000900B
 // public static public const SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900C
 // public static public const SAMPLER_CUBE_MAP_ARRAY_SHADOW As int  = 0x0000900D
 // public static public const INT_SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900E
 // public static public const UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900F
 // public static public const DRAW_INDIRECT_BUFFER As int  = 0x00008F3F
 // public static public const DRAW_INDIRECT_BUFFER_BINDING As int  = 0x00008F43
 // public static public const GEOMETRY_SHADER_INVOCATIONS As int  = 0x0000887F
 // public static public const MAX_GEOMETRY_SHADER_INVOCATIONS As int  = 0x00008E5A
 // public static public const MIN_FRAGMENT_INTERPOLATION_OFFSET As int  = 0x00008E5B
 // public static public const MAX_FRAGMENT_INTERPOLATION_OFFSET As int  = 0x00008E5C
 // public static public const FRAGMENT_INTERPOLATION_OFFSET_BITS As int  = 0x00008E5D
 // public static public const MAX_VERTEX_STREAMS As int  = 0x00008E71
 // public static public const DOUBLE_VEC2 As int  = 0x00008FFC
 // public static public const DOUBLE_VEC3 As int  = 0x00008FFD
 // public static public const DOUBLE_VEC4 As int  = 0x00008FFE
 // public static public const DOUBLE_MAT2 As int  = 0x00008F46
 // public static public const DOUBLE_MAT3 As int  = 0x00008F47
 // public static public const DOUBLE_MAT4 As int  = 0x00008F48
 // public static public const DOUBLE_MAT2x3 As int  = 0x00008F49
 // public static public const DOUBLE_MAT2x4 As int  = 0x00008F4A
 // public static public const DOUBLE_MAT3x2 As int  = 0x00008F4B
 // public static public const DOUBLE_MAT3x4 As int  = 0x00008F4C
 // public static public const DOUBLE_MAT4x2 As int  = 0x00008F4D
 // public static public const DOUBLE_MAT4x3 As int  = 0x00008F4E
 // public static public const ACTIVE_SUBROUTINES As int  = 0x00008DE5
 // public static public const ACTIVE_SUBROUTINE_UNIFORMS As int  = 0x00008DE6
 // public static public const ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS As int  = 0x00008E47
 // public static public const ACTIVE_SUBROUTINE_MAX_LENGTH As int  = 0x00008E48
 // public static public const ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH As int  = 0x00008E49
 // public static public const MAX_SUBROUTINES As int  = 0x00008DE7
 // public static public const MAX_SUBROUTINE_UNIFORM_LOCATIONS As int  = 0x00008DE8
 // public static public const NUM_COMPATIBLE_SUBROUTINES As int  = 0x00008E4A
 // public static public const COMPATIBLE_SUBROUTINES As int  = 0x00008E4B
 // public static public const PATCHES As int  = 0x0000000E
 // public static public const PATCH_VERTICES As int  = 0x00008E72
 // public static public const PATCH_DEFAULT_INNER_LEVEL As int  = 0x00008E73
 // public static public const PATCH_DEFAULT_OUTER_LEVEL As int  = 0x00008E74
 // public static public const TESS_CONTROL_OUTPUT_VERTICES As int  = 0x00008E75
 // public static public const TESS_GEN_MODE As int  = 0x00008E76
 // public static public const TESS_GEN_SPACING As int  = 0x00008E77
 // public static public const TESS_GEN_VERTEX_ORDER As int  = 0x00008E78
 // public static public const TESS_GEN_POINT_MODE As int  = 0x00008E79
 // public static public const ISOLINES As int  = 0x00008E7A
 // public static public const FRACTIONAL_ODD As int  = 0x00008E7B
 // public static public const FRACTIONAL_EVEN As int  = 0x00008E7C
 // public static public const MAX_PATCH_VERTICES As int  = 0x00008E7D
 // public static public const MAX_TESS_GEN_LEVEL As int  = 0x00008E7E
 // public static public const MAX_TESS_CONTROL_UNIFORM_COMPONENTS As int  = 0x00008E7F
 // public static public const MAX_TESS_EVALUATION_UNIFORM_COMPONENTS As int  = 0x00008E80
 // public static public const MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS As int  = 0x00008E81
 // public static public const MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS As int  = 0x00008E82
 // public static public const MAX_TESS_CONTROL_OUTPUT_COMPONENTS As int  = 0x00008E83
 // public static public const MAX_TESS_PATCH_COMPONENTS As int  = 0x00008E84
 // public static public const MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS As int  = 0x00008E85
 // public static public const MAX_TESS_EVALUATION_OUTPUT_COMPONENTS As int  = 0x00008E86
 // public static public const MAX_TESS_CONTROL_UNIFORM_BLOCKS As int  = 0x00008E89
 // public static public const MAX_TESS_EVALUATION_UNIFORM_BLOCKS As int  = 0x00008E8A
 // public static public const MAX_TESS_CONTROL_INPUT_COMPONENTS As int  = 0x0000886C
 // public static public const MAX_TESS_EVALUATION_INPUT_COMPONENTS As int  = 0x0000886D
 // public static public const MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS As int  = 0x00008E1E
 // public static public const MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS As int  = 0x00008E1F
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x000084F0
 // public static public const UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x000084F1
 // public static public const TESS_EVALUATION_SHADER As int  = 0x00008E87
 // public static public const TESS_CONTROL_SHADER As int  = 0x00008E88
 // public static public const TRANSFORM_FEEDBACK As int  = 0x00008E22
 // public static public const TRANSFORM_FEEDBACK_BUFFER_PAUSED As int  = 0x00008E23
 // public static public const TRANSFORM_FEEDBACK_BUFFER_ACTIVE As int  = 0x00008E24
 // public static public const TRANSFORM_FEEDBACK_BINDING As int  = 0x00008E25
 // public static public const MAX_TRANSFORM_FEEDBACK_BUFFERS As int  = 0x00008E70
 //
 // public static public const VERSION_3_3 1
 // public static public const VERTEX_ATTRIB_ARRAY_DIVISOR As int  = 0x000088FE
 // public static public const SRC1_COLOR As int  = 0x000088F9
 // public static public const ONE_MINUS_SRC1_COLOR As int  = 0x000088FA
 // public static public const ONE_MINUS_SRC1_ALPHA As int  = 0x000088FB
 // public static public const MAX_DUAL_SOURCE_DRAW_BUFFERS As int  = 0x000088FC
 // public static public const ANY_SAMPLES_PASSED As int  = 0x00008C2F
 // public static public const SAMPLER_BINDING As int  = 0x00008919
 // public static public const RGB10_A2UI As int  = 0x0000906F
 // public static public const TEXTURE_SWIZZLE_R As int  = 0x00008E42
 // public static public const TEXTURE_SWIZZLE_G As int  = 0x00008E43
 // public static public const TEXTURE_SWIZZLE_B As int  = 0x00008E44
 // public static public const TEXTURE_SWIZZLE_A As int  = 0x00008E45
 // public static public const TEXTURE_SWIZZLE_RGBA As int  = 0x00008E46
 // public static public const TIME_ELAPSED As int  = 0x000088BF
 // public static public const TIMESTAMP As int  = 0x00008E28
 // public static public const INT_2_10_10_10_REV As int  = 0x00008D9F
 // public static public const FIXED As int  = 0x0000140C
 // public static public const IMPLEMENTATION_COLOR_READ_TYPE As int  = 0x00008B9A
 // public static public const IMPLEMENTATION_COLOR_READ_FORMAT As int  = 0x00008B9B
 // public static public const LOW_single As int  = 0x00008DF0
 // public static public const MEDIUM_single As int  = 0x00008DF1
 // public static public const HIGH_single As int  = 0x00008DF2
 // public static public const LOW_INT As int  = 0x00008DF3
 // public static public const MEDIUM_INT As int  = 0x00008DF4
 // public static public const HIGH_INT As int  = 0x00008DF5
 // public static public const SHADER_COMPILER As int  = 0x00008DFA
 // public static public const SHADER_BINARY_FORMATS As int  = 0x00008DF8
 // public static public const NUM_SHADER_BINARY_FORMATS As int  = 0x00008DF9
 // public static public const MAX_VERTEX_UNIFORM_VECTORS As int  = 0x00008DFB
 // public static public const MAX_VARYING_VECTORS As int  = 0x00008DFC
 // public static public const MAX_FRAGMENT_UNIFORM_VECTORS As int  = 0x00008DFD
 // public static public const RGB565 As int  = 0x00008D62
 // public static public const PROGRAM_BINARY_RETRIEVABLE_HINT As int  = 0x00008257
 // public static public const PROGRAM_BINARY_LENGTH As int  = 0x00008741
 // public static public const NUM_PROGRAM_BINARY_FORMATS As int  = 0x000087FE
 // public static public const PROGRAM_BINARY_FORMATS As int  = 0x000087FF
 // public static public const VERTEX_SHADER_BIT As int  = 0x000000000001
 // public static public const FRAGMENT_SHADER_BIT As int  = 0x000000000002
 // public static public const GEOMETRY_SHADER_BIT As int  = 0x000000000004
 // public static public const TESS_CONTROL_SHADER_BIT As int  = 0x000000000008
 // public static public const TESS_EVALUATION_SHADER_BIT As int  = 0x000000000010
 // public static public const ALL_SHADER_BITS As int  = 0x0000FFFFFFFF
 // public static public const PROGRAM_SEPARABLE As int  = 0x00008258
 // public static public const ACTIVE_PROGRAM As int  = 0x00008259
 // public static public const PROGRAM_PIPELINE_BINDING As int  = 0x0000825A
 // public static public const MAX_VIEWPORTS As int  = 0x0000825B
 // public static public const VIEWPORT_SUBPIXEL_BITS As int  = 0x0000825C
 // public static public const VIEWPORT_BOUNDS_RANGE As int  = 0x0000825D
 // public static public const LAYER_PROVOKING_VERTEX As int  = 0x0000825E
 // public static public const VIEWPORT_INDEX_PROVOKING_VERTEX As int  = 0x0000825F
 // public static public const UNDEFINED_VERTEX As int  = 0x00008260
 // public static public const VERSION_4_5 1
 // public static public const CONTEXT_LOST As int  = 0x00000507
 // public static public const NEGATIVE_ONE_TO_ONE As int  = 0x0000935E
 // public static public const ZERO_TO_ONE As int  = 0x0000935F
 // public static public const CLIP_ORIGIN As int  = 0x0000935C
 // public static public const CLIP_DEPTH_MODE As int  = 0x0000935D
 // public static public const QUERY_WAIT_INVERTED As int  = 0x00008E17
 // public static public const QUERY_NO_WAIT_INVERTED As int  = 0x00008E18
 // public static public const QUERY_BY_REGION_WAIT_INVERTED As int  = 0x00008E19
 // public static public const QUERY_BY_REGION_NO_WAIT_INVERTED As int  = 0x00008E1A
 // public static public const MAX_CULL_DISTANCES As int  = 0x000082F9
 // public static public const MAX_COMBINED_CLIP_AND_CULL_DISTANCES As int  = 0x000082FA
 // public static public const TEXTURE_TARGET As int  = 0x00001006
 // public static public const QUERY_TARGET As int  = 0x000082EA
 // public static public const GUILTY_CONTEXT_RESET As int  = 0x00008253
 // public static public const INNOCENT_CONTEXT_RESET As int  = 0x00008254
 // public static public const UNKNOWN_CONTEXT_RESET As int  = 0x00008255
 // public static public const RESET_NOTIFICATION_STRATEGY As int  = 0x00008256
 // public static public const LOSE_CONTEXT_ON_RESET As int  = 0x00008252
 // public static public const NO_RESET_NOTIFICATION As int  = 0x00008261
 // public static public const CONTEXT_FLAG_ROBUST_ACCESS_BIT As int  = 0x000000000004
 // public static public const CONTEXT_RELEASE_BEHAVIOR As int  = 0x000082FB
 // public static public const CONTEXT_RELEASE_BEHAVIOR_FLUSH As int  = 0x000082FC
 // public static public const Console.WriteLine(_OUTPUT_SYNCHRONOUS_ARB As int  = 0x00008242
 // public static public const Console.WriteLine(_NEXT_LOGGED_MESSAGE_LENGTH_ARB As int  = 0x00008243
 // public static public const Console.WriteLine(_CALLBACK_FUNCTION_ARB As int  = 0x00008244
 // public static public const Console.WriteLine(_CALLBACK_USER_PARAM_ARB As int  = 0x00008245
 // public static public const Console.WriteLine(_SOURCE_API_ARB As int  = 0x00008246
 // public static public const Console.WriteLine(_SOURCE_WINDOW_SYSTEM_ARB As int  = 0x00008247
 // public static public const Console.WriteLine(_SOURCE_SHADER_COMPILER_ARB As int  = 0x00008248
 // public static public const Console.WriteLine(_SOURCE_THIRD_PARTY_ARB As int  = 0x00008249
 // public static public const Console.WriteLine(_SOURCE_APPLICATION_ARB As int  = 0x0000824A
 // public static public const Console.WriteLine(_SOURCE_OTHER_ARB As int  = 0x0000824B
 // public static public const Console.WriteLine(_TYPE_ERROR_ARB As int  = 0x0000824C
 // public static public const Console.WriteLine(_TYPE_DEPRECATED_BEHAVIOR_ARB As int  = 0x0000824D
 // public static public const Console.WriteLine(_TYPE_UNDEFINED_BEHAVIOR_ARB As int  = 0x0000824E
 // public static public const Console.WriteLine(_TYPE_PORTABILITY_ARB As int  = 0x0000824F
 // public static public const Console.WriteLine(_TYPE_PERFORMANCE_ARB As int  = 0x00008250
 // public static public const Console.WriteLine(_TYPE_OTHER_ARB As int  = 0x00008251
 // public static public const MAX_Console.WriteLine(_MESSAGE_LENGTH_ARB As int  = 0x00009143
 // public static public const MAX_Console.WriteLine(_LOGGED_MESSAGES_ARB As int  = 0x00009144
 // public static public const Console.WriteLine(_LOGGED_MESSAGES_ARB As int  = 0x00009145
 // public static public const Console.WriteLine(_SEVERITY_HIGH_ARB As int  = 0x00009146
 // public static public const Console.WriteLine(_SEVERITY_MEDIUM_ARB As int  = 0x00009147
 // public static public const Console.WriteLine(_SEVERITY_LOW_ARB As int  = 0x00009148
 // public static public const ARB_geometry_shader4 1
 // public static public const LINES_ADJACENCY_ARB As int  = 0x0000000A
 // public static public const LINE_STRIP_ADJACENCY_ARB As int  = 0x0000000B
 // public static public const TRIANGLES_ADJACENCY_ARB As int  = 0x0000000C
 // public static public const TRIANGLE_STRIP_ADJACENCY_ARB As int  = 0x0000000D
 // public static public const PROGRAM_POINT_SIZE_ARB As int  = 0x00008642
 // public static public const MAX_GEOMETRY_TEXTURE_IMAGE_UNITS_ARB As int  = 0x00008C29
 // public static public const FRAMEBUFFER_ATTACHMENT_LAYERED_ARB As int  = 0x00008DA7
 // public static public const FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS_ARB As int  = 0x00008DA8
 // public static public const FRAMEBUFFER_INCOMPLETE_LAYER_COUNT_ARB As int  = 0x00008DA9
 // public static public const GEOMETRY_SHADER_ARB As int  = 0x00008DD9
 // public static public const GEOMETRY_VERTICES_OUT_ARB As int  = 0x00008DDA
 // public static public const GEOMETRY_INPUT_TYPE_ARB As int  = 0x00008DDB
 // public static public const GEOMETRY_OUTPUT_TYPE_ARB As int  = 0x00008DDC
 // public static public const MAX_GEOMETRY_VARYING_COMPONENTS_ARB As int  = 0x00008DDD
 // public static public const MAX_VERTEX_VARYING_COMPONENTS_ARB As int  = 0x00008DDE
 // public static public const MAX_GEOMETRY_UNIFORM_COMPONENTS_ARB As int  = 0x00008DDF
 // public static public const MAX_GEOMETRY_OUTPUT_VERTICES_ARB As int  = 0x00008DE0
 // public static public const MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS_ARB As int  = 0x00008DE1
 //
 // public static public const ARB_gpu_shader_int64 1
 // public static public const INT64_ARB As int  = 0x0000140E
 // public static public const INT64_VEC2_ARB As int  = 0x00008FE9
 // public static public const INT64_VEC3_ARB As int  = 0x00008FEA
 // public static public const INT64_VEC4_ARB As int  = 0x00008FEB
 // public static public const UNSIGNED_INT64_VEC2_ARB As int  = 0x00008FF5
 // public static public const UNSIGNED_INT64_VEC3_ARB As int  = 0x00008FF6
 // public static public const UNSIGNED_INT64_VEC4_ARB As int  = 0x00008FF7
 // public static public const VERSION_4_4 1
 // public static public const MAX_VERTEX_ATTRIB_STRIDE As int  = 0x000082E5
 // public static public const PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED As int  = 0x00008221
 // public static public const TEXTURE_BUFFER_BINDING As int  = 0x00008C2A
 // public static public const MAP_PERSISTENT_BIT As int  = 0x00000040
 // public static public const MAP_COHERENT_BIT As int  = 0x00000080
 // public static public const DYNAMIC_STORAGE_BIT As int  = 0x00000100
 // public static public const CLIENT_STORAGE_BIT As int  = 0x00000200
 // public static public const CLIENT_MAPPED_BUFFER_BARRIER_BIT As int  = 0x000000004000
 // public static public const BUFFER_IMMUTABLE_STORAGE As int  = 0x0000821F
 // public static public const BUFFER_STORAGE_FLAGS As int  = 0x00008220
 // public static public const CLEAR_TEXTURE As int  = 0x00009365
 // public static public const LOCATION_COMPONENT As int  = 0x0000934A
 // public static public const TRANSFORM_FEEDBACK_BUFFER_INDEX As int  = 0x0000934B
 // public static public const TRANSFORM_FEEDBACK_BUFFER_STRIDE As int  = 0x0000934C
 // public static public const QUERY_BUFFER As int  = 0x00009192
 // public static public const QUERY_BUFFER_BARRIER_BIT As int  = 0x000000008000
 // public static public const QUERY_BUFFER_BINDING As int  = 0x00009193
 // public static public const QUERY_RESULT_NO_WAIT As int  = 0x00009194
 // public static public const MIRROR_CLAMP_TO_EDGE As int  = 0x00008743
 // public static public const VERSION_4_6 1
 // public static public const SHADER_BINARY_FORMAT_SPIR_V As int  = 0x00009551
 // public static public const SPIR_V_BINARY As int  = 0x00009552
 // public static public const PARAMETER_BUFFER As int  = 0x000080EE
 // public static public const PARAMETER_BUFFER_BINDING As int  = 0x000080EF
 // public static public const CONTEXT_FLAG_NO_ERROR_BIT As int  = 0x000000000008
 // public static public const VERTICES_SUBMITTED As int  = 0x000082EE
 // public static public const PRIMITIVES_SUBMITTED As int  = 0x000082EF
 // public static public const VERTEX_SHADER_INVOCATIONS As int  = 0x000082F0
 // public static public const TESS_CONTROL_SHADER_PATCHES As int  = 0x000082F1
 // public static public const TESS_EVALUATION_SHADER_INVOCATIONS As int  = 0x000082F2
 // public static public const GEOMETRY_SHADER_PRIMITIVES_EMITTED As int  = 0x000082F3
 // public static public const FRAGMENT_SHADER_INVOCATIONS As int  = 0x000082F4
 // public static public const COMPUTE_SHADER_INVOCATIONS As int  = 0x000082F5
 // public static public const CLIPPING_INPUT_PRIMITIVES As int  = 0x000082F6
 // public static public const CLIPPING_OUTPUT_PRIMITIVES As int  = 0x000082F7
 // public static public const POLYGON_OFFSET_CLAMP As int  = 0x00008E1B
 // public static public const SPIR_V_EXTENSIONS As int  = 0x00009553
 // public static public const NUM_SPIR_V_EXTENSIONS As int  = 0x00009554
 // public static public const TEXTURE_MAX_ANISOTROPY As int  = 0x000084FE
 // public static public const MAX_TEXTURE_MAX_ANISOTROPY As int  = 0x000084FF
 // public static public const TRANSFORM_FEEDBACK_OVERFLOW As int  = 0x000082EC
 // public static public const TRANSFORM_FEEDBACK_STREAM_OVERFLOW As int  = 0x000082ED
 // public static public const ARB_compute_variable_group_size 1
 // public static public const MAX_COMPUTE_VARIABLE_GROUP_INVOCATIONS_ARB As int  = 0x00009344
 // public static public const MAX_COMPUTE_FIXED_GROUP_INVOCATIONS_ARB As int  = 0x000090EB
 // public static public const MAX_COMPUTE_VARIABLE_GROUP_SIZE_ARB As int  = 0x00009345
 // public static public const MAX_COMPUTE_FIXED_GROUP_SIZE_ARB As int  = 0x000091BF

// public static void glCullFace(int Mode)
//     {

// public static void glFrontFace(int Mode)
//     {

// public static void glHint(int iTarget, int Mode)
//     {

// public static void glLineWidth(double fWIdth)
//     {

// public static void glPointSize(double fSize)
//     {

// public static void glPolygonMode(int face, int Mode)
//     {

// public static void glScissor(int x, int y, int iWIdth, int iHeight)
//     {

// public static void glTexParameterf(int iTarget, int iPname, double fParam)
//     {

// public static void glTexParameterfv(int iTarget, int iPname, double[] slxParams)
//     {

// public static void glTexParameteri(int iTarget, int iPname, int iParam)
//     {

// public static void glTexParameteriv(int iTarget, int iPname, int[] inxParams)
//     {

// public static void glTexImage1D(int iTarget, int iLevel, int iInternalFormat, int iWIdth, int iBorder, int iFormat, int iType, Byte[] pixels) // verificar si funciona byte
//     {

// public static void glTexImage2D(int iTarget, int iLevel, int iInternalFormat, int iWIdth, int iHeight, int iBorder, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glDrawBuffer(int iBuff)
//     {

// public static void glClear(int GiMaskBitField)
//     {

// public static void glClearColor(double red, double green, double blue, double alpha)
//     {

// public static void glClearStencil(int s)
//     {

// public static void glClearDepth(double depth)
//     {

// public static void glStencilMask(int mask)
//     {

// public static void glColorMask(Byte red, Byte green, Byte blue, Byte alpha)
//     {

// public static void glDepthMask(Byte flag)
//     {

// public static void glDisable(int cap)
//     {

// public static void glEnable(int cap)
//     {

// public static void glFinish()
//     {

// public static void glFlush()
//     {

// public static void glBlendFunc(int sFactor, int dFactor)
//     {

// public static void glLogicOp(int opcode)
//     {

// public static void glStencilFunc(int func, int ref, int mask)
//     {

// public static void glStencilOp(int fail, int zfail, int zpass)
//     {

// public static void glDepthFunc(int func)
//     {

// public static void glPixelStoref(int iPname, double fParam)
//     {

// public static void glPixelStorei(int iPname, int iParam)
//     {

// public static void glReadBuffer(int src)
//     {

// public static void glReadPixels(int x, int y, int iWIdth, int iHeight, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glGetBooleanv(int iPname, Byte[] data)
//     {

// public static void glGetDoublev(int iPname, double[] data)
//     {

// public static int glGetError()
//     {

// public static void glGetsinglev(int iPname, double[] data)
//     {

// public static void glGetintv(int iPname, int[] data)
//     {

// public static string glGetString(int name)
//     {

// public static void glGetTexImage(int iTarget, int iLevel, int iFormat, int iType, Byte pixels)
//     {

// public static void glGetTexParameterfv(int iTarget, int iPname, double[] params)
//     {

// public static void glGetTexParameteriv(int iTarget, int iPname, int[] params)
//     {

// public static void glGetTexLevelParameterfv(int iTarget, int iLevel, int iPname, double[] params)
//     {

// public static void glGetTexLevelParameteriv(int iTarget, int iLevel, int iPname, int[] params)
//     {

// public static  Byte glIsEnabled(int cap)
//     {

// public static void glDepthRange(double n, double f)
//     {

// public static void glViewport(int x, int y, int iWIdth, int iHeight)
//     {


// public static void glDrawArrays(int Mode, int first, int iSize)
//     {

// public static void glDrawElements(int Mode, int iSize, int iType, Byte[] indices) // verificar si es byte
//     {

// public static void glGetPointerv(int iPname, Pointer params)
//     {

// public static void glPolygonOffset(double factor, double units)
//     {

// public static void glCopyTexImage1D(int iTarget, int iLevel, int internalformat, int x, int y, int iWIdth, int iBorder)
//     {

// public static void glCopyTexImage2D(int iTarget, int iLevel, int internalformat, int x, int y, int iWIdth, int iHeight, int iBorder)
//     {

// public static void glCopyTexSubImage1D(int iTarget, int iLevel, int xoffset, int x, int y, int iWIdth)
//     {

// public static void glCopyTexSubImage2D(int iTarget, int iLevel, int xoffset, int yoffset, int x, int y, int iWIdth, int iHeight)
//     {

// public static void glTexSubImage1D(int iTarget, int iLevel, int xoffset, int iWIdth, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glTexSubImage2D(int iTarget, int iLevel, int xOffset, int yOffset, int iWIdth, int iHeight, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glBindTexture(int iTarget, int texture)
//     {

// public static void glDeleteTextures(int n, int[] textures)
//     {

// public static void glGenTextures(int n, int[] textures)
//     {

// public static bool glIsTexture(int texture)
//     {


// public static void glDrawRangeElements(int Mode, int start, int iEnd, int iSize, int iType, Byte[] indices)
//     {

// public static void glTexImage3D(int iTarget, int iLevel, int iInternalFormat, int iWIdth, int iHeight, int iDepth, int iBorder, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glTexSubImage3D(int iTarget, int iLevel, int xOffset, int yOffset, int zOffset, int iWIdth, int iHeight, int iDepth, int iFormat, int iType, Byte[] pixels)
//     {

// public static void glCopyTexSubImage3D(int iTarget, int iLevel, int xOffset, int yOffset, int zOffset, int x, int y, int iWIdth, int iHeight)
//     {


// public static void glActiveTexture(int texture)
//     {

// public static void glSampleCoverage(double value, Byte invert)
//     {

// public static void glCompressedTexImage3D(int iTarget, int iLevel, int internalformat, int iWIdth, int iHeight, int iDepth, int iBorder, int imagesize, Byte[] data)
//     {

// public static void glCompressedTexImage2D(int iTarget, int iLevel, int internalformat, int iWIdth, int iHeight, int iBorder, int imagesize, Byte[] data)
//     {

// public static void glCompressedTexImage1D(int iTarget, int iLevel, int internalformat, int iWIdth, int iBorder, int imagesize, Byte[] data)
//     {

// public static void glCompressedTexSubImage3D(int iTarget, int iLevel, int xOffset, int yOffset, int zOffset, int iWIdth, int iHeight, int iDepth, int iFormat, int imagesize, Byte[] data)
//     {

// public static void glCompressedTexSubImage2D(int iTarget, int iLevel, int xOffset, int yOffset, int iWIdth, int iHeight, int iFormat, int imagesize, Byte[] data)
//     {

// public static void glCompressedTexSubImage1D(int iTarget, int iLevel, int xOffset, int iWIdth, int iFormat, int imagesize, Byte[] data)
//     {

// public static void glGetCompressedTexImage(int iTarget, int iLevel, Byte[] img)
//     {


// public static void glBlendFuncSeparate(int sFactorRGB, int dFactorRGB, int sFactorAlpha, int dFactorAlpha)
//     {

// public static void glMultiDrawArrays(int Mode, int[] first, int[] count, int drawcount)
//     {

// public static void glMultiDrawElements(int Mode, int[] count, int iType, int[][] indices, int drawcount)
//     {

// public static void glPointParameterf(int iPname, double fParam)
//     {

// public static void glPointParameterfv(int iPname, double[] flxParams)
//     {

// public static void glPointParameteri(int iPname, int iParam)
//     {

// public static void glPointParameteriv(int iPname, int[] inxParams)
//     {

// public static void glBlendColor(double red, double green, double blue, double alpha)
//     {

// public static void glBlendEquation(int Mode)
//     {


// public static void glGenQueries(int n, int[] Ids)
//     {

// public static void glDeleteQueries(int n, int[] Ids)
//     {

// public static bool glIsQuery(int Id)
//     {

// public static void glBeginQuery(int iTarget, int Id)
//     {

// public static void glEndQuery(int iTarget)
//     {

// public static void glGetQueryiv(int iTarget, int iPname, int[] params)
//     {

// public static void glGetQueryObjectiv(int Id, int iPname, int[] params)
//     {

// public static void glGetQueryObjectuiv(int Id, int iPname, int[] params)
//     {

// public static void glBindBuffer(int iTarget, int buffer)
//     {

// public static void glDeleteBuffers(int n, Pointer buffers)
//     {


//  // Calling with (n as int, VarPtr(inx.data))  with inx.count >= n
// public static void glGenBuffers(int n, Pointer buffers)
//     {

// public static bool glIsBuffer(int buffer)
//     {

// public static void glBufferData(int iTarget, long size, Pointer data, int usage)
//     {

// public static void glBufferSubData(int iTarget, int offset, long size, Pointer data)
//     {

// public static void glGetBufferSubData(int iTarget, int offset, long size, Pointer data)
//     {

// public static int[] glMapBuffer(int iTarget, int access)
//     {

// public static bool glUnmapBuffer(int iTarget)
//     {

// public static void glGetBufferParameteriv(int iTarget, int iPname, Pointer params)
//     {

// public static void glGetBufferPointerv(int iTarget, int iPname, Pointer params)
//     {


// public static void glBlendEquationSeparate(int ModeRGB, int Modealpha)
//     {

// public static void glDrawBuffers(int n, int[] buffers)
//     {

// public static void glStencilOpSeparate(int face, int sFail, int dpFail, int dpPass)
//     {

// public static void glStencilFuncSeparate(int face, int func, int ref, int mask)
//     {

// public static void glStencilMaskSeparate(int face, int mask)
//     {

// public static void glAttachShader(int program, int shader)
//     {

// public static void glBindAttribLocation(int program, int iIndex, string name)
//     {

// public static void glCompileShader(int shader)
//     {

// public static int glCreateProgram()
//     {

// public static int glCreateShader(int iType)
//     {

// public static void glDeleteProgram(int program)
//     {

// public static void glDeleteShader(int shader)
//     {

// public static void glDetachShader(int program, int shader)
//     {

// public static void glDisableVertexAttribArray(int iIndex)
//     {

// public static void glEnableVertexAttribArray(int iIndex)
//     {

// public static void glGetActiveAttrib(int program, int iIndex, int BufSize, int[] length, int[] size, int[] type, string name)
//     {

// public static void glGetActiveUniform(int program, int iIndex, int BufSize, int[] length, int[] size, int[] type, string name)
//     {

// public static void glGetAttachedShaders(int program, int MaxCount, int[] count, int[] shaders)
//     {

// public static void glGetAttribLocation(int program, string name)
//     {

// public static void glGetProgramiv(int program, int iPname, int[] params)
//     {

// public static void glGetProgramInfoLog(int program, int BufSize, int[] length, string infoLog)
//     {

// public static void glGetShaderiv(int shader, int iPname, int[] params)
//     {

// public static void glGetShaderInfoLog(int shader, int BufSize, int[] legth, string infolog)
//     {

// public static void glGetShaderSource(int shader, int BufSize, int[] legth, string source)
//     {

// public static void glGetUniformLocation(int program, string name)
//     {

// public static void glGetUniformfv(int program, int location, double[] params)
//     {

// public static void glGetUniformiv(int program, int location, int[] params)
//     {

// public static void glGetVertexAttribdv(int iIndex, int iPname, double[] params)
//     {

// public static void glGetVertexAttribfv(int iIndex, int iPname, double[] params)
//     {

// public static void glGetVertexAttribiv(int iIndex, int iPname, int[] params)
//     {

// public static void glGetVertexAttribPointerv(int iIndex, int iPname, Pointer p)
//     {

// public static bool glIsProgram(int program)
//     {

// public static bool glIsShader(int shader)
//     {

// public static void glLinkProgram(int program)
//     {

// public static void glShaderSource(int shader, int iSize, string ByRef Source, int ByRef length)
//     {

// public static void glUseProgram(int program)
//     {

// public static void glUniform1f(int location, double v0)
//     {

// public static void glUniform2f(int location, double v0, double v1)
//     {

// public static void glUniform3f(int location, double v0, double v1, double v2)
//     {

// public static void glUniform4f(int location, double v0, double v1, double v2, double v3)
//     {

// public static void glUniform1i(int location, int v0)
//     {

// public static void glUniform2i(int location, int v0, int v1)
//     {

// public static void glUniform3i(int location, int v0, int v1, int v2)
//     {

// public static void glUniform4i(int location, int v0, int v1, int v2, int v3)
//     {

// public static void glUniform1fv(int location, int iSize, double[] value)
//     {

// public static void glUniform2fv(int location, int iSize, double[] value)
//     {

// public static void glUniform3fv(int location, int iSize, double[] value)
//     {

// public static void glUniform4fv(int location, int iSize, double[] value)
//     {

// public static void glUniform1iv(int location, int iSize, int[] value)
//     {

// public static void glUniform2iv(int location, int iSize, int[] value)
//     {

// public static void glUniform3iv(int location, int iSize, int[] value)
//     {

// public static void glUniform4iv(int location, int iSize, int[] value)
//     {

//  // public static Extern glUniformMatrix2fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
//  // public static Extern glUniformMatrix3fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
//  // public static Extern glUniformMatrix4fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
// public static void glValIdateProgram(int program)
//     {

 // public static Extern glVertexAttrib1d(iIndex As int, x As double)
 // public static Extern glVertexAttrib1dv(iIndex As int, public const As double * v)
 // public static Extern glVertexAttrib1f(iIndex As int, GLsingle x)
 // public static Extern glVertexAttrib1fv(iIndex As int, public const GLsingle * v)
 // public static Extern glVertexAttrib1s(iIndex As int, GLshort x)
 // public static Extern glVertexAttrib1sv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttrib2d(iIndex As int, x As double, As double y)
 // public static Extern glVertexAttrib2dv(iIndex As int, public const As double * v)
 // public static Extern glVertexAttrib2f(iIndex As int, GLsingle x, GLsingle y)
 // public static Extern glVertexAttrib2fv(iIndex As int, public const GLsingle * v)
 // public static Extern glVertexAttrib2s(iIndex As int, GLshort x, GLshort y)
 // public static Extern glVertexAttrib2sv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttrib3d(iIndex As int, x As double, As double y, As double z)
 // public static Extern glVertexAttrib3dv(iIndex As int, public const As double * v)
 // public static Extern glVertexAttrib3f(iIndex As int, GLsingle x, GLsingle y, GLsingle z)
 // public static Extern glVertexAttrib3fv(iIndex As int, public const GLsingle * v)
 // public static Extern glVertexAttrib3s(iIndex As int, GLshort x, GLshort y, GLshort z)
 // public static Extern glVertexAttrib3sv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttrib4Nbv(iIndex As int, public const GLbyte * v)
 // public static Extern glVertexAttrib4Niv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttrib4Nsv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttrib4Nub(iIndex As int, GLubyte x, GLubyte y, GLubyte z, GLubyte w)
 // public static Extern glVertexAttrib4Nubv(iIndex As int, public const GLubyte * v)
 // public static Extern glVertexAttrib4Nuiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttrib4Nusv(iIndex As int, public const GLushort * v)
 // public static Extern glVertexAttrib4bv(iIndex As int, public const GLbyte * v)
 // public static Extern glVertexAttrib4d(iIndex As int, x As double, As double y, As double z, As double w)
 // public static Extern glVertexAttrib4dv(iIndex As int, public const As double * v)
 // public static Extern glVertexAttrib4f(iIndex As int, GLsingle x, GLsingle y, GLsingle z, GLsingle w)
 // public static Extern glVertexAttrib4fv(iIndex As int, public const GLsingle * v)
 // public static Extern glVertexAttrib4iv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttrib4s(iIndex As int, GLshort x, GLshort y, GLshort z, GLshort w)
 // public static Extern glVertexAttrib4sv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttrib4ubv(iIndex As int, public const GLubyte * v)
 // public static Extern glVertexAttrib4uiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttrib4usv(iIndex As int, public const GLushort * v)
 // public static Extern glVertexAttribPointer(iIndex As int, size As int, iType As int, normalized As Boolean, stride As int, ipointer As Byte[]) // FIXME: verificar

 // public static Extern glUniformMatrix2x3fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
 // public static Extern glUniformMatrix3x2fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
 // public static Extern glUniformMatrix2x4fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
 // public static Extern glUniformMatrix4x2fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
 // public static Extern glUniformMatrix3x4fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)
 // public static Extern glUniformMatrix4x3fv(location As int, iSize As int, As Byte transpose, public const GLsingle * value)

 // public static Extern glColorMaski(iIndex As int, as byte r, as byte g, as byte b, as byte a)
 // public static Extern glGetBooleani_v(iTarget As int, iIndex As int, as byte data as byte[])
 // public static Extern glGetinti_v(iTarget As int, iIndex As int, GLint data as byte[])
 // public static Extern glEnablei(iTarget As int, iIndex As int)
 // public static Extern glDisablei(iTarget As int, iIndex As int)
 // GLAPI as byte APIENTRY glIsEnabledi(iTarget As int, iIndex As int)
 // public static Extern glBeginTransformFeedback(as int primitiveMode)
 // public static Extern glEndTransformFeedback()
 // public static Extern glBindBufferRange(iTarget As int, iIndex As int, buffer as int, offset as int, size as int)
 // public static Extern glBindBufferBase(iTarget As int, iIndex As int, buffer as int)
 // public static Extern glTransformFeedbackVaryings(program As int, iSize as int, public const GLchar * public const * varyings, iBuff as intferMode)
 // public static Extern glGetTransformFeedbackVarying(program As int, iIndex As int, BufSize As int, legth as int[], GLsizei * size, as int * type, name as string)
 // public static Extern glClampColor(iTarget As int, as int clamp)
 // public static Extern glBeginConditionalRender(id as int, Mode As int)
 // public static Extern glEndConditionalRender()
 // public static Extern glVertexAttribIPointer(iIndex As int, size as int, iType As int, stride as int, public const * pointer)
 // public static Extern glGetVertexAttribIiv(iIndex As int, iPname As int, params as int[])
 // public static Extern glGetVertexAttribIuiv(iIndex As int, iPname As int, params as int[])
 // public static Extern glVertexAttribI1i(iIndex As int, x As int)
 // public static Extern glVertexAttribI2i(iIndex As int, x As int, y As int)
 // public static Extern glVertexAttribI3i(iIndex As int, x As int, y As int, GLint z)
 // public static Extern glVertexAttribI4i(iIndex As int, x As int, y As int, GLint z, GLint w)
 // public static Extern glVertexAttribI1ui(iIndex As int, GLuint x)
 // public static Extern glVertexAttribI2ui(iIndex As int, GLuint x, GLuint y)
 // public static Extern glVertexAttribI3ui(iIndex As int, GLuint x, GLuint y, GLuint z)
 // public static Extern glVertexAttribI4ui(iIndex As int, GLuint x, GLuint y, GLuint z, GLuint w)
 // public static Extern glVertexAttribI1iv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttribI2iv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttribI3iv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttribI4iv(iIndex As int, public const GLint * v)
 // public static Extern glVertexAttribI1uiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttribI2uiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttribI3uiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttribI4uiv(iIndex As int, public const GLuint * v)
 // public static Extern glVertexAttribI4bv(iIndex As int, public const GLbyte * v)
 // public static Extern glVertexAttribI4sv(iIndex As int, public const GLshort * v)
 // public static Extern glVertexAttribI4ubv(iIndex As int, public const GLubyte * v)
 // public static Extern glVertexAttribI4usv(iIndex As int, public const GLushort * v)
 // public static Extern glGetUniformuiv(program As int, location as int, params as int[])
 // public static Extern glBindFragDataLocation(program As int, GLuint color, public const name as string)
 // public static Extern Function glGetFragDataLocation(program As int, public const name as string)
 // public static Extern glUniform1ui(location as int, GLuint v0)
 // public static Extern glUniform2ui(location as int, GLuint v0, GLuint v1)
 // public static Extern glUniform3ui(location as int, GLuint v0, GLuint v1, GLuint v2)
 // public static Extern glUniform4ui(location as int, GLuint v0, GLuint v1, GLuint v2, GLuint v3)
 // public static Extern glUniform1uiv(location as int, iSize as int, public const GLuint * value)
 // public static Extern glUniform2uiv(location as int, iSize as int, public const GLuint * value)
 // public static Extern glUniform3uiv(location as int, iSize as int, public const GLuint * value)
 // public static Extern glUniform4uiv(location as int, iSize as int, public const GLuint * value)
 // public static Extern glTexParameterIiv(iTarget As int, iPname As int, inxParams as int[])
 // public static Extern glTexParameterIuiv(iTarget As int, iPname As int, public const params as int[])
 // public static Extern glGetTexParameterIiv(iTarget As int, iPname As int, params as int[])
 // public static Extern glGetTexParameterIuiv(iTarget As int, iPname As int, params as int[])
 // public static Extern glClearBufferiv(iBuff as intfer, GLint drawbuffer, value as int[])
 // public static Extern glClearBufferuiv(iBuff as intfer, GLint drawbuffer, public const GLuint * value)
 // public static Extern glClearBufferfv(iBuff as intfer, GLint drawbuffer, public const GLsingle * value)
 // public static Extern glClearBufferfi(iBuff as intfer, GLint drawbuffer, GLsingle depth, GLint stencil)
 // GLAPI public const GLubyte * APIENTRY glGetStringi(as int name, iIndex As int)
 // GLAPI as byte APIENTRY glIsRenderbuffer(GLuint renderbuffer)
 // public static Extern glBindRenderbuffer(iTarget As int, GLuint renderbuffer)
 // public static Extern glDeleteRenderbuffers(n as int, public const GLuint * renderbuffers)
 // public static Extern glGenRenderbuffers(n as int, GLuint * renderbuffers)
 // public static Extern glRenderbufferStorage(iTarget As int,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glGetRenderbufferParameteriv(iTarget As int, iPname As int, params as int[])
 // GLAPI as byte APIENTRY glIsFramebuffer(GLuint framebuffer)
 // public static Extern glBindFramebuffer(iTarget As int, GLuint framebuffer)
 // public static Extern glDeleteFramebuffers(n as int, public const GLuint * framebuffers)
 // public static Extern glGenFramebuffers(n as int, GLuint * framebuffers)
 // GLAPI as int APIENTRY glCheckFramebufferStatus(iTarget As int)
 // public static Extern glFramebufferTexture1D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int)
 // public static Extern glFramebufferTexture2D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int)
 // public static Extern glFramebufferTexture3D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int, zOffset as int)
 // public static Extern glFramebufferRenderbuffer(iTarget As int, as int attachment, as int renderbuffertarget, GLuint renderbuffer)
 // public static Extern glGetFramebufferAttachmentParameteriv(iTarget As int, as int attachment, iPname As int, params as int[])
 // public static Extern glGenerateMipmap(iTarget As int)
 // public static Extern glBlitFramebuffer(GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GiMaskBitField as int, as int filter)
 // public static Extern glRenderbufferStorageMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glFramebufferTextureLayer(iTarget As int, as int attachment, texture as int, iLevel as int, GLint layer)
 // GLAPI * APIENTRY glMapBufferRange(iTarget As int, offset as int, GLsizeiptr length, GLbitfield access)
 // public static Extern glFlushMappedBufferRange(iTarget As int, offset as int, GLsizeiptr length)
 // public static Extern glBindVertexArray(GLuint array)
 // public static Extern glDeleteVertexArrays(n as int, public const GLuint * arrays)
 // public static Extern glGenVertexArrays(n as int, GLuint * arrays)
 // GLAPI as byte APIENTRY glIsVertexArray(GLuint array)

 // public static Extern glDrawArraysInstanced(Mode As int, first as int, iSize as int, GLsizei instancecount)
 // public static Extern glDrawElementsInstanced(Mode As int, iSize as int, iType As int, public const * indices, GLsizei instancecount)
 // public static Extern glTexBuffer(iTarget As int,internalformat as int, buffer as int)
 // public static Extern glPrimitiveRestartIndex(iIndex As int)
 // public static Extern glCopyBufferSubData(as int readTarget, as int writeTarget, GLintptr readOffset, GLintptr writeOffset, size as int)
 // public static Extern glGetUniformIndices(program As int, GLsizei uniformCount, public const GLchar * public const * uniformNames, GLuint * uniformIndices)
 // public static Extern glGetActiveUniformsiv(program As int, GLsizei uniformCount, public const GLuint * uniformIndices, iPname As int, params as int[])
 // public static Extern glGetActiveUniformName(program As int, GLuint uniformIndex, BufSize As int, legth as int[], GLchar * uniformName)
 // GLAPI GLuint APIENTRY glGetUniformBlockIndex(program As int, public const GLchar * uniformBlockName)
 // public static Extern glGetActiveUniformBlockiv(program As int, GLuint uniformBlockIndex, iPname As int, params as int[])
 // public static Extern glGetActiveUniformBlockName(program As int, GLuint uniformBlockIndex, BufSize As int, legth as int[], GLchar * uniformBlockName)
 // public static Extern glUniformBlockBinding(program As int, GLuint uniformBlockIndex, GLuint uniformBlockBinding)
 //

 // public static Extern glDrawElementsBaseVertex(Mode As int, iSize as int, iType As int, public const * indices, GLint basevertex)
 // public static Extern glDrawRangeElementsBaseVertex(Mode As int, start as int, iEnd as int , iSize as int, iType As int, public const * indices, GLint basevertex)
 // public static Extern glDrawElementsInstancedBaseVertex(Mode As int, iSize as int, iType As int, public const * indices, GLsizei instancecount, GLint basevertex)
 // public static Extern glMultiDrawElementsBaseVertex(Mode As int, public const GLsizei * count, iType As int, public const * public const * indices, drawcount as int, public const GLint * basevertex)
 // public static Extern glProvokingVertex(Mode As int)
 // GLAPI GLsync APIENTRY glFenceSync(as int condition, GLbitfield flags)
 // GLAPI as byte APIENTRY glIsSync(GLsync sync)
 // public static Extern glDeleteSync(GLsync sync)
 // GLAPI as int APIENTRY glClientWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)
 // public static Extern glWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)
 // public static Extern glGetint64v(iPname As int, GLint64 data as byte[])
 // public static Extern glGetSynciv(GLsync sync, iPname As int, BufSize As int, legth as int[], GLint * values)
 // public static Extern glGetint64i_v(iTarget As int, iIndex As int, GLint64 data as byte[])
 // public static Extern glGetBufferParameteri64v(iTarget As int, iPname As int, GLint64 * params)
 // public static Extern glFramebufferTexture(iTarget As int, as int attachment, texture as int, iLevel as int)
 // public static Extern glTexImage2DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // public static Extern glTexImage3DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // public static Extern glGetMultisamplefv(iPname As int, iIndex As int, GLsingle * val)
 // public static Extern glSampleMaski(mask as intNumber, GiMaskBitField as int)

 // public static Extern glBindFragDataLocationIndexed(program As int, GLuint colorNumber, iIndex As int, public const name as string)
 // public static Extern Function glGetFragDataIndex(program As int, public const name as string)
 // public static Extern glGenSamplers(iSize as int, GLuint * samplers)
 // public static Extern glDeleteSamplers(iSize as int, public const GLuint * samplers)
 // GLAPI as byte APIENTRY glIsSampler(GLuint sampler)
 // public static Extern glBindSampler(GLuint unit, GLuint sampler)
 // public static Extern glSamplerParameteri(GLuint sampler, iPname As int, iParam As int)
 // public static Extern glSamplerParameteriv(GLuint sampler, iPname As int, public const GLint * param)
 // public static Extern glSamplerParameterf(GLuint sampler, iPname As int, fParam As single)
 // public static Extern glSamplerParameterfv(GLuint sampler, iPname As int, public const GLsingle * param)
 // public static Extern glSamplerParameterIiv(GLuint sampler, iPname As int, public const GLint * param)
 // public static Extern glSamplerParameterIuiv(GLuint sampler, iPname As int, public const GLuint * param)
 // public static Extern glGetSamplerParameteriv(GLuint sampler, iPname As int, params as int[])
 // public static Extern glGetSamplerParameterIiv(GLuint sampler, iPname As int, params as int[])
 // public static Extern glGetSamplerParameterfv(GLuint sampler, iPname As int, params as single[])
 // public static Extern glGetSamplerParameterIuiv(GLuint sampler, iPname As int, params as int[])
 // public static Extern glQueryCounter(id as int, iTarget As int)
 // public static Extern glGetQueryObjecti64v(id as int, iPname As int, GLint64 * params)
 // public static Extern glGetQueryObjectui64v(id as int, iPname As int, GLuint64 * params)
 // public static Extern glVertexAttribDivisor(iIndex As int, GLuint divisor)
 // public static Extern glVertexAttribP1ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // public static Extern glVertexAttribP1uiv(iIndex As int, iType As int, as byte normalized, public const GLuint * value)
 // public static Extern glVertexAttribP2ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // public static Extern glVertexAttribP2uiv(iIndex As int, iType As int, as byte normalized, public const GLuint * value)
 // public static Extern glVertexAttribP3ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // public static Extern glVertexAttribP3uiv(iIndex As int, iType As int, as byte normalized, public const GLuint * value)
 // public static Extern glVertexAttribP4ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // public static Extern glVertexAttribP4uiv(iIndex As int, iType As int, as byte normalized, public const GLuint * value)
 //

 // public static Extern glMinSampleShading(value as single)
 // public static Extern glBlendEquationi(GLuint buf, Mode As int)
 // public static Extern glBlendEquationSeparatei(GLuint buf, Mode As intRGB, Mode As intAlpha)
 // public static Extern glBlendFunci(GLuint buf, as int src, as int dst)
 // public static Extern glBlendFuncSeparatei(GLuint buf, as int srcRGB, as int dstRGB, as int srcAlpha, as int dstAlpha)
 // public static Extern glDrawArraysIndirect(Mode As int, public const * indirect)
 // public static Extern glDrawElementsIndirect(Mode As int, iType As int, public const * indirect)
 // public static Extern glUniform1d(location as int, x as double)
 // public static Extern glUniform2d(location as int, x as double, as double y)
 // public static Extern glUniform3d(location as int, x as double, as double y, as double z)
 // public static Extern glUniform4d(location as int, x as double, as double y, as double z, as double w)
 // public static Extern glUniform1dv(location as int, iSize as int, public const as double * value)
 // public static Extern glUniform2dv(location as int, iSize as int, public const as double * value)
 // public static Extern glUniform3dv(location as int, iSize as int, public const as double * value)
 // public static Extern glUniform4dv(location as int, iSize as int, public const as double * value)
 // public static Extern glUniformMatrix2dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix3dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix4dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix2x3dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix2x4dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix3x2dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix3x4dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix4x2dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glUniformMatrix4x3dv(location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glGetUniformdv(program As int, location as int, as double * params)
 // public static Extern Function glGetSubroutineUniformLocation(program As int, as int shadertype, public const name as string)
 // GLAPI GLuint APIENTRY glGetSubroutineIndex(program As int, as int shadertype, public const name as string)
 // public static Extern glGetActiveSubroutineUniformiv(program As int, as int shadertype, iIndex As int, iPname As int, GLint * values)
 // public static Extern glGetActiveSubroutineUniformName(program As int, as int shadertype, iIndex As int, BufSize As int, legth as int[], name as string)
 // public static Extern glGetActiveSubroutineName(program As int, as int shadertype, iIndex As int, BufSize As int, legth as int[], name as string)
 // public static Extern glUniformSubroutinesuiv(as int shadertype, iSize as int, public const GLuint * indices)
 // public static Extern glGetUniformSubroutineuiv(as int shadertype, location as int, params as int[])
 // public static Extern glGetProgramStageiv(program As int, as int shadertype, iPname As int, GLint * values)
 // public static Extern glPatchParameteri(iPname As int, GLint value)
 // public static Extern glPatchParameterfv(iPname As int, public const GLsingle * values)
 // public static Extern glBindTransformFeedback(iTarget As int, id as int)
 // public static Extern glDeleteTransformFeedbacks(n as int, ids as int[])
 // public static Extern glGenTransformFeedbacks(n as int, GLuint * ids)
 // GLAPI as byte APIENTRY glIsTransformFeedback(id as int)
 // public static Extern glPauseTransformFeedback()
 // public static Extern glResumeTransformFeedback()
 // public static Extern glDrawTransformFeedback(Mode As int, id as int)
 // public static Extern glDrawTransformFeedbackStream(Mode As int, id as int, GLuint stream)
 // public static Extern glBeginQueryIndexed(iTarget As int, iIndex As int, id as int)
 // public static Extern glEndQueryIndexed(iTarget As int, iIndex As int)
 // public static Extern glGetQueryIndexediv(iTarget As int, iIndex As int, iPname As int, params as int[])
 //

 // public static Extern glReleaseShaderCompiler()
 // public static Extern glShaderBinary(iSize as int, public const GLuint * shaders, as int binaryformat, public const * Binary , GLsizei length)
 // public static Extern glGetShaderPrecisionFormat(as int shadertype, as int precisiontype, GLint * range, GLint * precision)
 // public static Extern glDepthRangef(GLsingle n, GLsingle f)
 // public static Extern glClearDepthf(GLsingle d)
 // public static Extern glGetProgramBinary(program As int, BufSize As int, legth as int[], as int * binaryFormat, * Binary )
 // public static Extern glProgramBinary(program As int, as int binaryFormat, public const * Binary , GLsizei length)
 // public static Extern glProgramParameteri(program As int, iPname As int, GLint value)
 // public static Extern glUseProgramStages(GLuint pipeline, GLbitfield stages, program As int)
 // public static Extern glActiveShaderProgram(GLuint pipeline, program As int)
 // GLAPI GLuint APIENTRY glCreateShaderProgramv(iType As int, iSize as int, public const GLchar * public const * strings)
 // public static Extern glBindProgramPipeline(GLuint pipeline)
 // public static Extern glDeleteProgramPipelines(n as int, public const GLuint * pipelines)
 // public static Extern glGenProgramPipelines(n as int, GLuint * pipelines)
 // GLAPI as byte APIENTRY glIsProgramPipeline(GLuint pipeline)
 // public static Extern glGetProgramPipelineiv(GLuint pipeline, iPname As int, params as int[])
 // public static Extern glProgramUniform1i(program As int, location as int, v0 as int)
 // public static Extern glProgramUniform1iv(program As int, location as int, iSize as int, value as int[])
 // public static Extern glProgramUniform1f(program As int, location as int, v0 as single)
 // public static Extern glProgramUniform1fv(program As int, location as int, iSize as int, public const GLsingle * value)
 // public static Extern glProgramUniform1d(program As int, location as int, as double v0)
 // public static Extern glProgramUniform1dv(program As int, location as int, iSize as int, public const as double * value)
 // public static Extern glProgramUniform1ui(program As int, location as int, GLuint v0)
 // public static Extern glProgramUniform1uiv(program As int, location as int, iSize as int, public const GLuint * value)
 // public static Extern glProgramUniform2i(program As int, location as int, v0 as int, v1 as int)
 // public static Extern glProgramUniform2iv(program As int, location as int, iSize as int, value as int[])
 // public static Extern glProgramUniform2f(program As int, location as int, v0 as single, v1 as single)
 // public static Extern glProgramUniform2fv(program As int, location as int, iSize as int, public const GLsingle * value)
 // public static Extern glProgramUniform2d(program As int, location as int, as double v0, as double v1)
 // public static Extern glProgramUniform2dv(program As int, location as int, iSize as int, public const as double * value)
 // public static Extern glProgramUniform2ui(program As int, location as int, GLuint v0, GLuint v1)
 // public static Extern glProgramUniform2uiv(program As int, location as int, iSize as int, public const GLuint * value)
 // public static Extern glProgramUniform3i(program As int, location as int, v0 as int, v1 as int, v2 as int)
 // public static Extern glProgramUniform3iv(program As int, location as int, iSize as int, value as int[])
 // public static Extern glProgramUniform3f(program As int, location as int, v0 as single, v1 as single, v2 as single)
 // public static Extern glProgramUniform3fv(program As int, location as int, iSize as int, public const GLsingle * value)
 // public static Extern glProgramUniform3d(program As int, location as int, as double v0, as double v1, as double v2)
 // public static Extern glProgramUniform3dv(program As int, location as int, iSize as int, public const as double * value)
 // public static Extern glProgramUniform3ui(program As int, location as int, GLuint v0, GLuint v1, GLuint v2)
 // public static Extern glProgramUniform3uiv(program As int, location as int, iSize as int, public const GLuint * value)
 // public static Extern glProgramUniform4i(program As int, location as int, v0 as int, v1 as int, v2 as int, v3 as int)
 // public static Extern glProgramUniform4iv(program As int, location as int, iSize as int, value as int[])
 // public static Extern glProgramUniform4f(program As int, location as int, v0 as single, v1 as single, v2 as single, v3 as single)
 // public static Extern glProgramUniform4fv(program As int, location as int, iSize as int, public const GLsingle * value)
 // public static Extern glProgramUniform4d(program As int, location as int, as double v0, as double v1, as double v2, as double v3)
 // public static Extern glProgramUniform4dv(program As int, location as int, iSize as int, public const as double * value)
 // public static Extern glProgramUniform4ui(program As int, location as int, GLuint v0, GLuint v1, GLuint v2, GLuint v3)
 // public static Extern glProgramUniform4uiv(program As int, location as int, iSize as int, public const GLuint * value)
 // public static Extern glProgramUniformMatrix2fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix3fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix4fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix2dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix3dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix4dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix2x3fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix3x2fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix2x4fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix4x2fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix3x4fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix4x3fv(program As int, location as int, iSize as int, as byte transpose, public const GLsingle * value)
 // public static Extern glProgramUniformMatrix2x3dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix3x2dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix2x4dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix4x2dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix3x4dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glProgramUniformMatrix4x3dv(program As int, location as int, iSize as int, as byte transpose, public const as double * value)
 // public static Extern glValidateProgramPipeline(GLuint pipeline)
 // public static Extern glGetProgramPipelineInfoLog(GLuint pipeline, BufSize As int, legth as int[], GLchar * infoLog)
 // public static Extern glVertexAttribL1d(iIndex As int, x as double)
 // public static Extern glVertexAttribL2d(iIndex As int, x as double, as double y)
 // public static Extern glVertexAttribL3d(iIndex As int, x as double, as double y, as double z)
 // public static Extern glVertexAttribL4d(iIndex As int, x as double, as double y, as double z, as double w)
 // public static Extern glVertexAttribL1dv(iIndex As int, public const as double * v)
 // public static Extern glVertexAttribL2dv(iIndex As int, public const as double * v)
 // public static Extern glVertexAttribL3dv(iIndex As int, public const as double * v)
 // public static Extern glVertexAttribL4dv(iIndex As int, public const as double * v)
 // public static Extern glVertexAttribLPointer(iIndex As int, size as int, iType As int, stride as int, public const * pointer)
 // public static Extern glGetVertexAttribLdv(iIndex As int, iPname As int, as double * params)
 // public static Extern glViewportArrayv(GLuint first, iSize as int, public const GLsingle * v)
 // public static Extern glViewportIndexedf(iIndex As int, GLsingle x, GLsingle y, GLsingle w, GLsingle h)
 // public static Extern glViewportIndexedfv(iIndex As int, public const GLsingle * v)
 // public static Extern glScissorArrayv(GLuint first, iSize as int, public const GLint * v)
 // public static Extern glScissorIndexed(iIndex As int, GLint left, GLint bottom, iWidth As int, iHeight As int)
 // public static Extern glScissorIndexedv(iIndex As int, public const GLint * v)
 // public static Extern glDepthRangeArrayv(GLuint first, iSize as int, public const as double * v)
 // public static Extern glDepthRangeIndexed(iIndex As int, as double n, as double f)
 // public static Extern glGetsinglei_v(iTarget As int, iIndex As int, GLsingle data as byte[])
 // public static Extern glGetDoublei_v(iTarget As int, iIndex As int, as double data as byte[])
 //

 // public static Extern glDrawArraysInstancedBaseInstance(Mode As int, first as int, iSize as int, GLsizei instancecount, GLuint baseinstance)
 // public static Extern glDrawElementsInstancedBaseInstance(Mode As int, iSize as int, iType As int, public const * indices, GLsizei instancecount, GLuint baseinstance)
 // public static Extern glDrawElementsInstancedBaseVertexBaseInstance(Mode As int, iSize as int, iType As int, public const * indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance)
 // public static Extern glGetInternalformativ(iTarget As int,internalformat as int, iPname As int, BufSize As int, params as int[])
 // public static Extern glGetActiveAtomicCounterBufferiv(program As int, buffer as intIndex, iPname As int, params as int[])
 // public static Extern glBindImageTexture(GLuint unit, texture as int, iLevel as int, as byte layered, GLint layer, as int access, iFormat as int)
 // public static Extern glMemoryBarrier(GLbitfield barriers)
 // public static Extern glTexStorage1D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int)
 // public static Extern glTexStorage2D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glTexStorage3D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int, iDepth as int)
 // public static Extern glDrawTransformFeedbackInstanced(Mode As int, id as int, GLsizei instancecount)
 // public static Extern glDrawTransformFeedbackStreamInstanced(Mode As int, id as int, GLuint stream, GLsizei instancecount)

 // public static Extern glClearBufferData(iTarget As int,internalformat as int, iFormat as int, iType As int, data as byte[])
 // public static Extern glClearBufferSubData(iTarget As int,internalformat as int, offset as int, size as int, iFormat as int, iType As int, data as byte[])
 // public static Extern glDispatchCompute(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z)
 // public static Extern glDispatchComputeIndirect(GLintptr indirect)
 // public static Extern glCopyImageSubData(GLuint srcName, as int srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, as int dstTarget, GLint dstLevel, GLint dstX, GLint dstY, GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth)
 // public static Extern glFramebufferParameteri(iTarget As int, iPname As int, iParam As int)
 // public static Extern glGetFramebufferParameteriv(iTarget As int, iPname As int, params as int[])
 // public static Extern glGetInternalformati64v(iTarget As int,internalformat as int, iPname As int, BufSize As int, GLint64 * params)
 // public static Extern glInvalidateTexSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int)
 // public static Extern glInvalidateTexImage(texture as int, iLevel as int)
 // public static Extern glInvalidateBufferSubData(buffer as int, offset as int, GLsizeiptr length)
 // public static Extern glInvalidateBufferData(buffer as int)
 // public static Extern glInvalidateFramebuffer(iTarget As int, n as intumAttachments, public const as int * attachments)
 // public static Extern glInvalidateSubFramebuffer(iTarget As int, n as intumAttachments, public const as int * attachments, x As int, y As int, iWidth As int, iHeight As int)
 // public static Extern glMultiDrawArraysIndirect(Mode As int, public const * indirect, drawcount as int, stride as int)
 // public static Extern glMultiDrawElementsIndirect(Mode As int, iType As int, public const * indirect, drawcount as int, stride as int)
 // public static Extern glGetProgramInterfaceiv(program As int, as int programInterface, iPname As int, params as int[])
 // GLAPI GLuint APIENTRY glGetProgramResourceIndex(program As int, as int programInterface, public const name as string)
 // public static Extern glGetProgramResourceName(program As int, as int programInterface, iIndex As int, BufSize As int, legth as int[], name as string)
 // public static Extern glGetProgramResourceiv(program As int, as int programInterface, iIndex As int, GLsizei propCount, public const as int * props, BufSize As int, legth as int[], params as int[])
 // public static Extern Function glGetProgramResourceLocation(program As int, as int programInterface, public const name as string)
 // public static Extern Function glGetProgramResourceLocationIndex(program As int, as int programInterface, public const name as string)
 // public static Extern glShaderStorageBlockBinding(program As int, GLuint storageBlockIndex, GLuint storageBlockBinding)
 // public static Extern glTexBufferRange(iTarget As int,internalformat as int, buffer as int, offset as int, size as int)
 // public static Extern glTexStorage2DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // public static Extern glTexStorage3DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // public static Extern glTextureView(texture as int, iTarget As int, GLuint origtexture,internalformat as int, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers)
 // public static Extern glBindVertexBuffer(GLuint bindingindex, buffer as int, offset as int, stride as int)
 // public static Extern glVertexAttribFormat(GLuint attribindex, size as int, iType As int, as byte normalized, GLuint relativeoffset)
 // public static Extern glVertexAttribIFormat(GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // public static Extern glVertexAttribLFormat(GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // public static Extern glVertexAttribBinding(GLuint attribindex, GLuint bindingindex)
 // public static Extern glVertexBindingDivisor(GLuint bindingindex, GLuint divisor)
 // public static Extern glConsole.WriteLine(MessageControl(as int source, iType As int, as int severity, iSize as int, ids as int[], as byte enabled)
 // public static Extern glConsole.WriteLine(MessageInsert(as int source, iType As int, id as int, as int severity, GLsizei length, public const GLchar * buf)
 // public static Extern glConsole.WriteLine(MessageCallback(GLConsole.WriteLine(PROC callback, public const * userParam)
 // GLAPI GLuint APIENTRY glGetConsole.WriteLine(MessageLog(iCount As int, BufSize As int, as int * sources, as int * types, GLuint * ids, as int * severities, legth as int[]s, GLchar * messageLog)
 // public static Extern glPushConsole.WriteLine(Group(as int source, id as int, GLsizei length, public const GLchar * message)
 // public static Extern glPopConsole.WriteLine(Group()
 // public static Extern glObjectLabel(as int identifier, GLuint name, GLsizei length, public const GLchar * label)
 // public static Extern glGetObjectLabel(as int identifier, GLuint name, BufSize As int, legth as int[], GLchar * label)
 // public static Extern glObjectPtrLabel(public const * ptr, GLsizei length, public const GLchar * label)
 // public static Extern glGetObjectPtrLabel(public const * ptr, BufSize As int, legth as int[], GLchar * label)

 // public static Extern glBufferStorage(iTarget As int, size as int, data as byte[], GLbitfield flags)
 // public static Extern glClearTexImage(texture as int, iLevel as int, iFormat as int, iType As int, data as byte[])
 // public static Extern glClearTexSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, data as byte[])
 // public static Extern glBindBuffersBase(iTarget As int, GLuint first, iSize as int, public const buffers as int[])
 // public static Extern glBindBuffersRange(iTarget As int, GLuint first, iSize as int, public const buffers as int[], public const GLintptr * offsets, public const GLsizeiptr * sizes)
 // public static Extern glBindTextures(GLuint first, iSize as int, public const GLuint * textures)
 // public static Extern glBindSamplers(GLuint first, iSize as int, public const GLuint * samplers)
 // public static Extern glBindImageTextures(GLuint first, iSize as int, public const GLuint * textures)
 // public static Extern glBindVertexBuffers(GLuint first, iSize as int, public const buffers as int[], public const GLintptr * offsets, public const GLsizei * strides)

 // public static Extern glClipControl(as int origin, as int depth)
 // public static Extern glCreateTransformFeedbacks(n as int, GLuint * ids)
 // public static Extern glTransformFeedbackBufferBase(GLuint xfb, iIndex As int, buffer as int)
 // public static Extern glTransformFeedbackBufferRange(GLuint xfb, iIndex As int, buffer as int, offset as int, size as int)
 // public static Extern glGetTransformFeedbackiv(GLuint xfb, iPname As int, GLint * param)
 // public static Extern glGetTransformFeedbacki_v(GLuint xfb, iPname As int, iIndex As int, GLint * param)
 // public static Extern glGetTransformFeedbacki64_v(GLuint xfb, iPname As int, iIndex As int, GLint64 * param)
 // public static Extern glCreateBuffers(n as int, buffers as int[])
 // public static Extern glNamedBufferStorage(buffer as int, size as int, data as byte[], GLbitfield flags)
 // public static Extern glNamedBufferData(buffer as int, size as int, data as byte[], as int usage)
 // public static Extern glNamedBufferSubData(buffer as int, offset as int, size as int, data as byte[])
 // public static Extern glCopyNamedBufferSubData(GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, size as int)
 // public static Extern glClearNamedBufferData(buffer as int,internalformat as int, iFormat as int, iType As int, data as byte[])
 // public static Extern glClearNamedBufferSubData(buffer as int,internalformat as int, offset as int, size as int, iFormat as int, iType As int, data as byte[])
 // GLAPI * APIENTRY glMapNamedBuffer(buffer as int, as int access)
 // GLAPI * APIENTRY glMapNamedBufferRange(buffer as int, offset as int, GLsizeiptr length, GLbitfield access)
 // GLAPI as byte APIENTRY glUnmapNamedBuffer(buffer as int)
 // public static Extern glFlushMappedNamedBufferRange(buffer as int, offset as int, GLsizeiptr length)
 // public static Extern glGetNamedBufferParameteriv(buffer as int, iPname As int, params as int[])
 // public static Extern glGetNamedBufferParameteri64v(buffer as int, iPname As int, GLint64 * params)
 // public static Extern glGetNamedBufferPointerv(buffer as int, iPname As int, * * params)
 // public static Extern glGetNamedBufferSubData(buffer as int, offset as int, size as int, data as byte[])
 // public static Extern glCreateFramebuffers(n as int, GLuint * framebuffers)
 // public static Extern glNamedFramebufferRenderbuffer(GLuint framebuffer, as int attachment, as int renderbuffertarget, GLuint renderbuffer)
 // public static Extern glNamedFramebufferParameteri(GLuint framebuffer, iPname As int, iParam As int)
 // public static Extern glNamedFramebufferTexture(GLuint framebuffer, as int attachment, texture as int, iLevel as int)
 // public static Extern glNamedFramebufferTextureLayer(GLuint framebuffer, as int attachment, texture as int, iLevel as int, GLint layer)
 // public static Extern glNamedFramebufferDrawBuffer(GLuint framebuffer, iBuff as int)
 // public static Extern glNamedFramebufferDrawBuffers(GLuint framebuffer, n as int, public const as int * bufs)
 // public static Extern glNamedFramebufferReadBuffer(GLuint framebuffer, as int src)
 // public static Extern glInvalidateNamedFramebufferData(GLuint framebuffer, n as intumAttachments, public const as int * attachments)
 // public static Extern glInvalidateNamedFramebufferSubData(GLuint framebuffer, n as intumAttachments, public const as int * attachments, x As int, y As int, iWidth As int, iHeight As int)
 // public static Extern glClearNamedFramebufferiv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, value as int[])
 // public static Extern glClearNamedFramebufferuiv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, public const GLuint * value)
 // public static Extern glClearNamedFramebufferfv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, public const GLsingle * value)
 // public static Extern glClearNamedFramebufferfi(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, GLsingle depth, GLint stencil)
 // public static Extern glBlitNamedFramebuffer(GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GiMaskBitField as int, as int filter)
 // GLAPI as int APIENTRY glCheckNamedFramebufferStatus(GLuint framebuffer, iTarget As int)
 // public static Extern glGetNamedFramebufferParameteriv(GLuint framebuffer, iPname As int, GLint * param)
 // public static Extern glGetNamedFramebufferAttachmentParameteriv(GLuint framebuffer, as int attachment, iPname As int, params as int[])
 // public static Extern glCreateRenderbuffers(n as int, GLuint * renderbuffers)
 // public static Extern glNamedRenderbufferStorage(GLuint renderbuffer,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glNamedRenderbufferStorageMultisample(GLuint renderbuffer, GLsizei samples,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glGetNamedRenderbufferParameteriv(GLuint renderbuffer, iPname As int, params as int[])
 // public static Extern glCreateTextures(iTarget As int, n as int, GLuint * textures)
 // public static Extern glTextureBuffer(texture as int,internalformat as int, buffer as int)
 // public static Extern glTextureBufferRange(texture as int,internalformat as int, buffer as int, offset as int, size as int)
 // public static Extern glTextureStorage1D(texture as int, GLsizei levels,internalformat as int, iWidth As int)
 // public static Extern glTextureStorage2D(texture as int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int)
 // public static Extern glTextureStorage3D(texture as int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int, iDepth as int)
 // public static Extern glTextureStorage2DMultisample(texture as int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // public static Extern glTextureStorage3DMultisample(texture as int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // public static Extern glTextureSubImage1D(texture as int, iLevel as int, xOffset as int, iWidth As int, iFormat as int, iType As int, pixels as byte[])
 // public static Extern glTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, iWidth As int, iHeight As int, iFormat as int, iType As int, pixels as byte[])
 // public static Extern glTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, pixels as byte[])
 // public static Extern glCompressedTextureSubImage1D(texture as int, iLevel as int, xOffset as int, iWidth As int, iFormat as int, imagesize as int, data as byte[])
 // public static Extern glCompressedTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, iWidth As int, iHeight As int, iFormat as int, imagesize as int, data as byte[])
 // public static Extern glCompressedTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, imagesize as int, data as byte[])
 // public static Extern glCopyTextureSubImage1D(texture as int, iLevel as int, xOffset as int, x As int, y As int, iWidth As int)
 // public static Extern glCopyTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, x As int, y As int, iWidth As int, iHeight As int)
 // public static Extern glCopyTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, x As int, y As int, iWidth As int, iHeight As int)
 // public static Extern glTextureParameterf(texture as int, iPname As int, fParam As single)
 // public static Extern glTextureParameterfv(texture as int, iPname As int, public const GLsingle * param)
 // public static Extern glTextureParameteri(texture as int, iPname As int, iParam As int)
 // public static Extern glTextureParameterIiv(texture as int, iPname As int, inxParams as int[])
 // public static Extern glTextureParameterIuiv(texture as int, iPname As int, public const params as int[])
 // public static Extern glTextureParameteriv(texture as int, iPname As int, public const GLint * param)
 // public static Extern glGenerateTextureMipmap(texture as int)
 // public static Extern glBindTextureUnit(GLuint unit, texture as int)
 // public static Extern glGetTextureImage(texture as int, iLevel as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // public static Extern glGetCompressedTextureImage(texture as int, iLevel as int, BufSize As int, * pixels)
 // public static Extern glGetTextureLevelParameterfv(texture as int, iLevel as int, iPname As int, params as single[])
 // public static Extern glGetTextureLevelParameteriv(texture as int, iLevel as int, iPname As int, params as int[])
 // public static Extern glGetTextureParameterfv(texture as int, iPname As int, params as single[])
 // public static Extern glGetTextureParameterIiv(texture as int, iPname As int, params as int[])
 // public static Extern glGetTextureParameterIuiv(texture as int, iPname As int, params as int[])
 // public static Extern glGetTextureParameteriv(texture as int, iPname As int, params as int[])
 // public static Extern glCreateVertexArrays(n as int, GLuint * arrays)
 // public static Extern glDisableVertexArrayAttrib(GLuint vaobj, iIndex As int)
 // public static Extern glEnableVertexArrayAttrib(GLuint vaobj, iIndex As int)
 // public static Extern glVertexArrayElementBuffer(GLuint vaobj, buffer as int)
 // public static Extern glVertexArrayVertexBuffer(GLuint vaobj, GLuint bindingindex, buffer as int, offset as int, stride as int)
 // public static Extern glVertexArrayVertexBuffers(GLuint vaobj, GLuint first, iSize as int, public const buffers as int[], public const GLintptr * offsets, public const GLsizei * strides)
 // public static Extern glVertexArrayAttribBinding(GLuint vaobj, GLuint attribindex, GLuint bindingindex)
 // public static Extern glVertexArrayAttribFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, as byte normalized, GLuint relativeoffset)
 // public static Extern glVertexArrayAttribIFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // public static Extern glVertexArrayAttribLFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // public static Extern glVertexArrayBindingDivisor(GLuint vaobj, GLuint bindingindex, GLuint divisor)
 // public static Extern glGetVertexArrayiv(GLuint vaobj, iPname As int, GLint * param)
 // public static Extern glGetVertexArrayIndexediv(GLuint vaobj, iIndex As int, iPname As int, GLint * param)
 // public static Extern glGetVertexArrayIndexed64iv(GLuint vaobj, iIndex As int, iPname As int, GLint64 * param)
 // public static Extern glCreateSamplers(n as int, GLuint * samplers)
 // public static Extern glCreateProgramPipelines(n as int, GLuint * pipelines)
 // public static Extern glCreateQueries(iTarget As int, n as int, GLuint * ids)
 // public static Extern glGetQueryBufferObjecti64v(id as int, buffer as int, iPname As int, offset as int)
 // public static Extern glGetQueryBufferObjectiv(id as int, buffer as int, iPname As int, offset as int)
 // public static Extern glGetQueryBufferObjectui64v(id as int, buffer as int, iPname As int, offset as int)
 // public static Extern glGetQueryBufferObjectuiv(id as int, buffer as int, iPname As int, offset as int)
 // public static Extern glMemoryBarrierByRegion(GLbitfield barriers)
 // public static Extern glGetTextureSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // public static Extern glGetCompressedTextureSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, BufSize As int, * pixels)
 // GLAPI as int APIENTRY glGetGraphicsResetStatus()
 // public static Extern glGetnCompressedTexImage(iTarget As int, GLint lod, BufSize As int, * pixels)
 // public static Extern glGetnTexImage(iTarget As int, iLevel as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // public static Extern glGetnUniformdv(program As int, location as int, BufSize As int, as double * params)
 // public static Extern glGetnUniformfv(program As int, location as int, BufSize As int, params as single[])
 // public static Extern glGetnUniformiv(program As int, location as int, BufSize As int, params as int[])
 // public static Extern glGetnUniformuiv(program As int, location as int, BufSize As int, params as int[])
 // public static Extern glReadnPixels(x As int, y As int, iWidth As int, iHeight As int, iFormat as int, iType As int, BufSize As int, data as byte[])
 // public static Extern glTextureBarrier()

 // Publicshader As intcializeShader(GLuint shader, public const GLchar * pEntryPoint, GLuint numSpecializationpublic static constants, public const GLuint * ppublic static constantIndex, public const GLuint * ppublic static constantValue)
 // public static Extern glMultiDrawArraysIndirectCount(Mode As int, public const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // public static Extern glMultiDrawElementsIndirectCount(Mode As int, iType As int, public const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // public static Extern glPolygonOffsetClamp(GLsingle factor, GLsingle units, GLsingle clamp)
 // public static public const GL_ARB_ES3_2_compatibility 1
 // public static public const GL_PRIMITIVE_BOUNDING_BOX_ARB As int = 0x92BE
 // public static public const GL_MULTISAMPLE_LINE_WIDTH_RANGE_ARB As int = 0x9381
 // public static public const GL_MULTISAMPLE_LINE_WIDTH_GRANULARITY_ARB As int = 0x9382
 // public static Extern glPrimitiveBoundingBoxARB(GLsingle minX, GLsingle minY, GLsingle minZ, GLsingle minW, GLsingle maxX, GLsingle maxY, GLsingle maxZ, GLsingle maxW)
 // GLAPI GLuint64 APIENTRY glGetTextureHandleARB(texture as int)
 // GLAPI GLuint64 APIENTRY glGetTextureSamplerHandleARB(texture as int, GLuint sampler)
 // public static Extern glMakeTextureHandleResidentARB(GLuint64 handle)
 // public static Extern glMakeTextureHandleNonResidentARB(GLuint64 handle)
 // GLAPI GLuint64 APIENTRY glGetImageHandleARB(texture as int, iLevel as int, as byte layered, GLint layer, iFormat as int)
 // public static Extern glMakeImageHandleResidentARB(GLuint64 handle, as int access)
 // public static Extern glMakeImageHandleNonResidentARB(GLuint64 handle)
 // public static Extern glUniformHandleui64ARB(location as int, GLuint64 value)
 // public static Extern glUniformHandleui64vARB(location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glProgramUniformHandleui64ARB(program As int, location as int, GLuint64 value)
 // public static Extern glProgramUniformHandleui64vARB(program As int, location as int, iSize as int, public const GLuint64 * values)
 // GLAPI as byte APIENTRY glIsTextureHandleResidentARB(GLuint64 handle)
 // GLAPI as byte APIENTRY glIsImageHandleResidentARB(GLuint64 handle)
 // public static Extern glVertexAttribL1ui64ARB(iIndex As int, GLuint64EXT x)
 // public static Extern glVertexAttribL1ui64vARB(iIndex As int, public const GLuint64EXT * v)
 // public static Extern glGetVertexAttribLui64vARB(iIndex As int, iPname As int, GLuint64EXT * params)
 // public static public const GL_ARB_cl_event 1
 // Struct _cl_context
 // Struct _cl_event
 // public static public const GL_SYNC_CL_EVENT_ARB As int = 0x8240
 // public static public const GL_SYNC_CL_EVENT_COMPLETE_ARB As int = 0x8241
 //
 // GLAPI GLsync APIENTRY glCreateSyncFromCLeventARB(struct _cl_context * context, struct _cl_event * Event , GLbitfield flags)

 // public static Extern glDispatchComputeGroupSizeARB(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z, GLuint group_size_x, GLuint group_size_y, GLuint group_size_z)
 //
 // public static public const GL_ARB_Console.WriteLine(_output 1
 // typedef(APIENTRY * GLConsole.WriteLine(PROCARB)(as int source, iType As int, id as int, as int severity, GLsizei length, public const GLchar * message, public const * userParam)

 // public static Extern glConsole.WriteLine(MessageControlARB(as int source, iType As int, as int severity, iSize as int, ids as int[], as byte enabled)
 // public static Extern glConsole.WriteLine(MessageInsertARB(as int source, iType As int, id as int, as int severity, GLsizei length, public const GLchar * buf)
 // public static Extern glConsole.WriteLine(MessageCallbackARB(GLConsole.WriteLine(PROCARB callback, public const * userParam)
 // GLAPI GLuint APIENTRY glGetConsole.WriteLine(MessageLogARB(iCount As int, BufSize As int, as int * sources, as int * types, GLuint * ids, as int * severities, legth as int[]s, GLchar * messageLog)
 //
 // public static public const GL_ARB_draw_buffers_blend 1
 //
 // public static Extern glBlendEquationiARB(GLuint buf, Mode As int)
 // public static Extern glBlendEquationSeparateiARB(GLuint buf, Mode As intRGB, Mode As intAlpha)
 // public static Extern glBlendFunciARB(GLuint buf, as int src, as int dst)
 // public static Extern glBlendFuncSeparateiARB(GLuint buf, as int srcRGB, as int dstRGB, as int srcAlpha, as int dstAlpha)
 //
 // public static public const GL_ARB_draw_instanced 1
 //
 // public static Extern glDrawArraysInstancedARB(Mode As int, first as int, iSize as int, GLsizei primcount)
 // public static Extern glDrawElementsInstancedARB(Mode As int, iSize as int, iType As int, public const * indices, GLsizei primcount)
 //

 // public static Extern glProgramParameteriARB(program As int, iPname As int, GLint value)
 // public static Extern glFramebufferTextureARB(iTarget As int, as int attachment, texture as int, iLevel as int)
 // public static Extern glFramebufferTextureLayerARB(iTarget As int, as int attachment, texture as int, iLevel as int, GLint layer)
 // public static Extern glFramebufferTextureFaceARB(iTarget As int, as int attachment, texture as int, iLevel as int, face As int)
 //
 // public static public const GL_ARB_gl_spirv 1
 // public static public const GL_SHADER_BINARY_FORMAT_SPIR_V_ARB As int = 0x9551
 // public static public const GL_SPIR_V_BINARY_ARB As int = 0x9552
 //
 // public static Extern glSpecializeShadshader As intshader, public const GLchar * pEntryPoint, GLuint numSpecializationpublic static constants, public const GLuint * ppublic static constantIndex, public const GLuint * ppublic static constantValue)
 //

 // public static Extern glUniform1i64ARB(location as int, GLint64 x)
 // public static Extern glUniform2i64ARB(location as int, GLint64 x, GLint64 y)
 // public static Extern glUniform3i64ARB(location as int, GLint64 x, GLint64 y, GLint64 z)
 // public static Extern glUniform4i64ARB(location as int, GLint64 x, GLint64 y, GLint64 z, GLint64 w)
 // public static Extern glUniform1i64vARB(location as int, iSize as int, public const GLint64 * value)
 // public static Extern glUniform2i64vARB(location as int, iSize as int, public const GLint64 * value)
 // public static Extern glUniform3i64vARB(location as int, iSize as int, public const GLint64 * value)
 // public static Extern glUniform4i64vARB(location as int, iSize as int, public const GLint64 * value)
 // public static Extern glUniform1ui64ARB(location as int, GLuint64 x)
 // public static Extern glUniform2ui64ARB(location as int, GLuint64 x, GLuint64 y)
 // public static Extern glUniform3ui64ARB(location as int, GLuint64 x, GLuint64 y, GLuint64 z)
 // public static Extern glUniform4ui64ARB(location as int, GLuint64 x, GLuint64 y, GLuint64 z, GLuint64 w)
 // public static Extern glUniform1ui64vARB(location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glUniform2ui64vARB(location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glUniform3ui64vARB(location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glUniform4ui64vARB(location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glGetUniformi64vARB(program As int, location as int, GLint64 * params)
 // public static Extern glGetUniformui64vARB(program As int, location as int, GLuint64 * params)
 // public static Extern glGetnUniformi64vARB(program As int, location as int, BufSize As int, GLint64 * params)
 // public static Extern glGetnUniformui64vARB(program As int, location as int, BufSize As int, GLuint64 * params)
 // public static Extern glProgramUniform1i64ARB(program As int, location as int, GLint64 x)
 // public static Extern glProgramUniform2i64ARB(program As int, location as int, GLint64 x, GLint64 y)
 // public static Extern glProgramUniform3i64ARB(program As int, location as int, GLint64 x, GLint64 y, GLint64 z)
 // public static Extern glProgramUniform4i64ARB(program As int, location as int, GLint64 x, GLint64 y, GLint64 z, GLint64 w)
 // public static Extern glProgramUniform1i64vARB(program As int, location as int, iSize as int, public const GLint64 * value)
 // public static Extern glProgramUniform2i64vARB(program As int, location as int, iSize as int, public const GLint64 * value)
 // public static Extern glProgramUniform3i64vARB(program As int, location as int, iSize as int, public const GLint64 * value)
 // public static Extern glProgramUniform4i64vARB(program As int, location as int, iSize as int, public const GLint64 * value)
 // public static Extern glProgramUniform1ui64ARB(program As int, location as int, GLuint64 x)
 // public static Extern glProgramUniform2ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y)
 // public static Extern glProgramUniform3ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y, GLuint64 z)
 // public static Extern glProgramUniform4ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y, GLuint64 z, GLuint64 w)
 // public static Extern glProgramUniform1ui64vARB(program As int, location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glProgramUniform2ui64vARB(program As int, location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glProgramUniform3ui64vARB(program As int, location as int, iSize as int, public const GLuint64 * value)
 // public static Extern glProgramUniform4ui64vARB(program As int, location as int, iSize as int, public const GLuint64 * value)
 //
 // public static public const GL_ARB_indirect_parameters 1
 // public static public const GL_PARAMETER_BUFFER_ARB As int = 0x80EE
 // public static public const GL_PARAMETER_BUFFER_BINDING_ARB As int = 0x80EF
 //
 // public static Extern glMultiDrawArraysIndirectCountARB(Mode As int, public const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // public static Extern glMultiDrawElementsIndirectCountARB(Mode As int, iType As int, public const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 //
 // public static public const GL_ARB_instanced_arrays 1
 // public static public const GL_VERTEX_ATTRIB_ARRAY_DIVISOR_ARB As int = 0x88FE
 //
 // public static Extern glVertexAttribDivisorARB(iIndex As int, GLuint divisor)
 //
 // public static public const GL_ARB_internalformat_query2 1
 // public static public const GL_SRGB_DECODE_ARB As int = 0x8299
 // public static public const GL_VIEW_CLASS_EAC_R11 As int = 0x9383
 // public static public const GL_VIEW_CLASS_EAC_RG11 As int = 0x9384
 // public static public const GL_VIEW_CLASS_ETC2_RGB As int = 0x9385
 // public static public const GL_VIEW_CLASS_ETC2_RGBA As int = 0x9386
 // public static public const GL_VIEW_CLASS_ETC2_EAC_RGBA As int = 0x9387
 // public static public const GL_VIEW_CLASS_ASTC_4x4_RGBA As int = 0x9388
 // public static public const GL_VIEW_CLASS_ASTC_5x4_RGBA As int = 0x9389
 // public static public const GL_VIEW_CLASS_ASTC_5x5_RGBA As int = 0x938A
 // public static public const GL_VIEW_CLASS_ASTC_6x5_RGBA As int = 0x938B
 // public static public const GL_VIEW_CLASS_ASTC_6x6_RGBA As int = 0x938C
 // public static public const GL_VIEW_CLASS_ASTC_8x5_RGBA As int = 0x938D
 // public static public const GL_VIEW_CLASS_ASTC_8x6_RGBA As int = 0x938E
 // public static public const GL_VIEW_CLASS_ASTC_8x8_RGBA As int = 0x938F
 // public static public const GL_VIEW_CLASS_ASTC_1 As int = & H5_RGBA As int = 0x9390
 // public static public const GL_VIEW_CLASS_ASTC_1 As int = & H6_RGBA As int = 0x9391
 // public static public const GL_VIEW_CLASS_ASTC_1 As int = & H8_RGBA As int = 0x9392
 // public static public const GL_VIEW_CLASS_ASTC_1 As int = & H10_RGBA As int = 0x9393
 // public static public const GL_VIEW_CLASS_ASTC_12x10_RGBA As int = 0x9394
 // public static public const GL_VIEW_CLASS_ASTC_12x12_RGBA As int = 0x9395
 //
 // public static public const GL_ARB_parallel_shader_compile 1
 // public static public const GL_MAX_SHADER_COMPILER_THREADS_ARB As int = 0x91B0
 // public static public const GL_COMPLETION_STATUS_ARB As int = 0x91B1
 // typedef(APIENTRYP PFNGLMAXSHADERCOMPILERTHREADSARBPROC)(iCount As int)
 //
 // public static Extern glMaxShaderCompilerThreadsARB(iCount As int)
 //
 // public static public const GL_ARB_pipeline_statistics_query 1
 // public static public const GL_VERTICES_SUBMITTED_ARB As int = 0x82EE
 // public static public const GL_PRIMITIVES_SUBMITTED_ARB As int = 0x82EF
 // public static public const GL_VERTEX_SHADER_INVOCATIONS_ARB As int = 0x82F0
 // public static public const GL_TESS_CONTROL_SHADER_PATCHES_ARB As int = 0x82F1
 // public static public const GL_TESS_EVALUATION_SHADER_INVOCATIONS_ARB As int = 0x82F2
 // public static public const GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED_ARB As int = 0x82F3
 // public static public const GL_FRAGMENT_SHADER_INVOCATIONS_ARB As int = 0x82F4
 // public static public const GL_COMPUTE_SHADER_INVOCATIONS_ARB As int = 0x82F5
 // public static public const GL_CLIPPING_INPUT_PRIMITIVES_ARB As int = 0x82F6
 // public static public const GL_CLIPPING_OUTPUT_PRIMITIVES_ARB As int = 0x82F7
 //
 // public static public const GL_ARB_pixel_buffer_object 1
 // public static public const GL_PIXEL_PACK_BUFFER_ARB As int = 0x88EB
 // public static public const GL_PIXEL_UNPACK_BUFFER_ARB As int = 0x88EC
 // public static public const GL_PIXEL_PACK_BUFFER_BINDING_ARB As int = 0x88ED
 // public static public const GL_PIXEL_UNPACK_BUFFER_BINDING_ARB As int = 0x88EF
 //
 // public static public const GL_ARB_robustness 1
 // public static public const GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT_ARB As int = 0x00000004
 // public static public const GL_LOSE_CONTEXT_ON_RESET_ARB As int = 0x8252
 // public static public const GL_GUILTY_CONTEXT_RESET_ARB As int = 0x8253
 // public static public const GL_INNOCENT_CONTEXT_RESET_ARB As int = 0x8254
 // public static public const GL_UNKNOWN_CONTEXT_RESET_ARB As int = 0x8255
 // public static public const GL_RESET_NOTIFICATION_STRATEGY_ARB As int = 0x8256
 // public static public const GL_NO_RESET_NOTIFICATION_ARB As int = 0x8261
 //
 // GLAPI as int APIENTRY glGetGraphicsResetStatusARB()
 // public static Extern glGetnTexImageARB(iTarget As int, iLevel as int, iFormat as int, iType As int, BufSize As int, * img)
 // public static Extern glReadnPixelsARB(x As int, y As int, iWidth As int, iHeight As int, iFormat as int, iType As int, BufSize As int, data as byte[])
 // public static Extern glGetnCompressedTexImageARB(iTarget As int, GLint lod, BufSize As int, * img)
 // public static Extern glGetnUniformfvARB(program As int, location as int, BufSize As int, params as single[])
 // public static Extern glGetnUniformivARB(program As int, location as int, BufSize As int, params as int[])
 // public static Extern glGetnUniformuivARB(program As int, location as int, BufSize As int, params as int[])
 // public static Extern glGetnUniformdvARB(program As int, location as int, BufSize As int, as double * params)
 //
 // public static public const GL_ARB_sample_locations 1
 // public static public const GL_SAMPLE_LOCATION_SUBPIXEL_BITS_ARB As int = 0x933D
 // public static public const GL_SAMPLE_LOCATION_PIXEL_GRID_WIDTH_ARB As int = 0x933E
 // public static public const GL_SAMPLE_LOCATION_PIXEL_GRID_HEIGHT_ARB As int = 0x933F
 // public static public const GL_PROGRAMMABLE_SAMPLE_LOCATION_TABLE_SIZE_ARB As int = 0x9340
 // public static public const GL_SAMPLE_LOCATION_ARB As int = 0x8E50
 // public static public const GL_PROGRAMMABLE_SAMPLE_LOCATION_ARB As int = 0x9341
 // public static public const GL_FRAMEBUFFER_PROGRAMMABLE_SAMPLE_LOCATIONS_ARB As int = 0x9342
 // public static public const GL_FRAMEBUFFER_SAMPLE_LOCATION_PIXEL_GRID_ARB As int = 0x9343
 //
 // public static Extern glFramebufferSampleLocationsfvARB(iTarget As int, start as int, iSize as int, public const GLsingle * v)
 // public static Extern glNamedFramebufferSampleLocationsfvARB(GLuint framebuffer, start as int, iSize as int, public const GLsingle * v)
 // public static Extern glEvaluateDepthValuesARB()
 //
 // public static public const GL_ARB_sample_shading 1
 // public static public const GL_SAMPLE_SHADING_ARB As int = 0x8C36
 // public static public const GL_MIN_SAMPLE_SHADING_VALUE_ARB As int = 0x8C37
 //
 // public static Extern glMinSampleShadingARB(value as single)
 //
 // public static public const GL_ARB_shading_language_include 1
 // public static public const GL_SHADER_INCLUDE_ARB As int = 0x8DAE
 // public static public const GL_NAMED_STRING_LENGTH_ARB As int = 0x8DE9
 // public static public const GL_NAMED_STRING_TYPE_ARB As int = 0x8DEA
 //
 // public static Extern glNamedStringARB(iType As int, GLint namelen, public const name as string, GLint stringlen, public const GLchar * string)
 // public static Extern glDeleteNamedStringARB(GLint namelen, public const name as string)
 // public static Extern glCompileShaderIncludeARB(GLuint shader, iSize as int, public const GLchar * public const * path, public const GLinshader As int
 // GLAPI as byte APIENTRY glIsNamedStringARB(GLint namelen, public const name as string)
 // public static Extern glGetNamedStringARB(GLint namelen, public const name as string, BufSize As int, GLint * stringlen, GLchar * string)
 // public static Extern glGetNamedStringivARB(GLint namelen, public const name as string, iPname As int, params as int[])
 //
 // public static public const GL_ARB_shading_language_packing 1
 //
 // public static public const GL_ARB_sparse_buffer 1
 // public static public const GL_SPARSE_STORAGE_BIT_ARB As int = 0x0400
 // public static public const GL_SPARSE_BUFFER_PAGE_SIZE_ARB As int = 0x82F8
 //
 // public static Extern glBufferPageCommitmentARB(iTarget As int, offset as int, size as int, as byte commit)
 // public static Extern glNamedBufferPageCommitmentEXT(buffer as int, offset as int, size as int, as byte commit)
 // public static Extern glNamedBufferPageCommitmentARB(buffer as int, offset as int, size as int, as byte commit)
 //
 // public static public const GL_ARB_sparse_texture 1
 // public static public const GL_TEXTURE_SPARSE_ARB As int = 0x91A6
 // public static public const GL_VIRTUAL_PAGE_SIZE_INDEX_ARB As int = 0x91A7
 // public static public const GL_NUM_SPARSE_LEVELS_ARB As int = 0x91AA
 // public static public const GL_NUM_VIRTUAL_PAGE_SIZES_ARB As int = 0x91A8
 // public static public const GL_VIRTUAL_PAGE_SIZE_X_ARB As int = 0x9195
 // public static public const GL_VIRTUAL_PAGE_SIZE_Y_ARB As int = 0x9196
 // public static public const GL_VIRTUAL_PAGE_SIZE_Z_ARB As int = 0x9197
 // public static public const GL_MAX_SPARSE_TEXTURE_SIZE_ARB As int = 0x9198
 // public static public const GL_MAX_SPARSE_3D_TEXTURE_SIZE_ARB As int = 0x9199
 // public static public const GL_MAX_SPARSE_ARRAY_TEXTURE_LAYERS_ARB As int = 0x919A
 // public static public const GL_SPARSE_TEXTURE_FULL_ARRAY_CUBE_MIPMAPS_ARB As int = 0x91A9
 //
 // public static Extern glTexPageCommitmentARB(iTarget As int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, as byte commit)
 // public static public const GL_ARB_sparse_texture2 1
 // public static public const GL_ARB_sparse_texture_clamp 1
 // public static public const GL_ARB_spirv_extensions 1
 // public static public const GL_ARB_stencil_texturing 1
 // public static public const GL_ARB_sync 1
 // public static public const GL_ARB_tessellation_shader 1
 // public static public const GL_ARB_texture_barrier 1
 // public static public const GL_ARB_texture_border_clamp 1
 // public static public const GL_CLAMP_TO_BORDER_ARB As int = 0x812D
 // public static public const GL_ARB_texture_buffer_object 1
 // public static public const GL_TEXTURE_BUFFER_ARB As int = 0x8C2A
 // public static public const GL_MAX_TEXTURE_BUFFER_SIZE_ARB As int = 0x8C2B
 // public static public const GL_TEXTURE_BINDING_BUFFER_ARB As int = 0x8C2C
 // public static public const GL_TEXTURE_BUFFER_DATA_STORE_BINDING_ARB As int = 0x8C2D
 // public static public const GL_TEXTURE_BUFFER_FORMAT_ARB As int = 0x8C2E
 // typedef(APIENTRYP PFNGLTEXBUFFERARBPROC)(iTarget As int,internalformat as int, buffer as int)
 // public static Extern glTexBufferARB(iTarget As int,internalformat as int, buffer as int)
 // public static public const GL_ARB_texture_buffer_object_rgb32 1
 // public static public const GL_ARB_texture_buffer_range 1
 // public static public const GL_ARB_texture_compression_bptc 1
 // public static public const GL_COMPRESSED_RGBA_BPTC_UNORM_ARB As int = 0x8E8C
 // public static public const GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB As int = 0x8E8D
 // public static public const GL_COMPRESSED_RGB_BPTC_SIGNED_single_ARB As int = 0x8E8E
 // public static public const GL_COMPRESSED_RGB_BPTC_UNSIGNED_single_ARB As int = 0x8E8F
 // public static public const GL_ARB_texture_compression_rgtc 1
 // public static public const GL_ARB_texture_cube_map_array 1
 // public static public const GL_TEXTURE_CUBE_MAP_ARRAY_ARB As int = 0x9009
 // public static public const GL_TEXTURE_BINDING_CUBE_MAP_ARRAY_ARB As int = 0x900A
 // public static public const GL_PROXY_TEXTURE_CUBE_MAP_ARRAY_ARB As int = 0x900B
 // public static public const GL_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900C
 // public static public const GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW_ARB As int = 0x900D
 // public static public const GL_INT_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900E
 // public static public const GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900F
 // public static public const GL_ARB_texture_filter_anisotropic 1
 //
 // public static public const GL_ARB_texture_filter_minmax 1
 // public static public const GL_TEXTURE_REDUCTION_MODE_ARB As int = 0x9366
 // public static public const GL_WEIGHTED_AVERAGE_ARB As int = 0x9367
 //
 // public static public const GL_ARB_texture_gather 1
 // public static public const GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET_ARB As int = 0x8E5E
 // public static public const GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET_ARB As int = 0x8E5F
 // public static public const GL_MAX_PROGRAM_TEXTURE_GATHER_COMPONENTS_ARB As int = 0x8F9F
 //
 // public static public const GL_ARB_transform_feedback_overflow_query 1
 // public static public const GL_TRANSFORM_FEEDBACK_OVERFLOW_ARB As int = 0x82EC
 // public static public const GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW_ARB As int = 0x82ED

 // ==============OTROS QUE NO ESTABAN (probablemente deprecados)=============================

// public static void glEnableClientState(int capacIdad)
//     {

// public static void glDisableClientState(int capacIdad)
//     {

// public static void glVertexPointer(int size, int type, long strIde, Pointer data)
//     {

// public static void glNormalPointer(int type, long strIde, Pointer data)
//     {

// public static void glTexCoordPointer(int size, int type, long strIde, Pointer data)
//     {

// public static void glColorPointer(int size, int type, long strIde, Pointer data)
//     {


 // Clears the VBO from memory
 // Clears the VBO from memory

 // Clears the VBO from memory


// public static void Resize(Object drwContext) // only glArea for now
//     {


//      // establecemos adonde vamos a dibujar, porque puede ser en algun lugar mas chico de la misma glDrawingArea
//      // pero en general sera en todo el control
//      // 2022 la siguiente linea parece estar deprecada, pongo cualquiera o simplemente la comento y funciona igual
//      //GL.Viewport(100, 100, drwContext.W / 2, drwContext.h / 2)

//     if ( ! Me.Initialized ) return; //Me.Init(drwContext)
//      //    Console.WriteLine( "New GL Sheet Init:", drwContext.w, drwContext.H
//      // le avisamos a GL que queremos usar texturas
//     GL.Enable(GL.TEXTURE_2D);

//      // borramos lo que haya dibujado
//     GL.Clear(GL.DEPTH_BUFFER_BIT || GL.COLOR_BUFFER_BIT);

//      // le decimos que queremos usar cosas 3D y lo que esta mas lejos quede tapado por lo que esta mas cerca, esto en 2D no es necesario
//     GL.Enable(GL.DEPTH_TEST);

//      // no tengo idea, debe ser algo de AntiAlias
//     GL.Enable(GL.SMOOTH);
//     GL.Enable(GL.BLEND);
//     GL.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
//      // usaremos luces
//      // GL.Enable(GL.LIGHTING)
//      // GL.Enable(GL.LIGHT0)
//      // esto le dice que las normales las tiene que normalizar a 1
//     GL.Enable(GL.NORMALIZE);

//      // las coordenadas que le pasemos deberan estar entre -1 y 1
//      // todo lo que sea > 1 NO SE MOSTRARA

//     GL.MatrixMode(GL.PROJECTION);
//     GL.LoadIdentity;
//      //Glu.Ortho2D(-drwContext.w / 2, drwContext.w / 2, -drwContext.h / 2, drwContext.h / 2)
//     GL.Ortho(-drwContext.w / 2, drwContext.w / 2, -drwContext.h / 2, drwContext.h / 2, 1, -1);

//     GL.MatrixMode(GL.MODELVIEW);
//     GL.LoadIdentity;

// }

// public static void Init(Object drwContext) // only glArea for now
//     {


//      //Resize(drwContext)
//     Console.WriteLine( "Init OpenGL");

//     int gbColor ;         
//     double r ;         
//     double g ;         
//     double b ;         
//     double a ;         

//     gbColor = drwContext.background;

//     a = 1 - Colors.GetAlpha(gbColor) / 255;
//     r = (Gb.Shr(gbColor, 16) & 255) / 255;
//     g = (Gb.Shr(gbColor, 8) & 255) / 255;
//     b = (gbColor & 255) / 255;

//     GL.ClearColor(r, g, b, a);

//      // line stipples

//     glFlush();
//     Console.WriteLine( "libGL access: ok");
//     Console.WriteLine( "Support VBO: " + GL.CheckExtensions("GL_ARB_vertex_buffer_object"));
//     Console.WriteLine( "Support GLSL: " + GL.CheckExtensions("GL_ARB_vertex_program"));
//     Console.WriteLine( "Shading version: " + GL.GetString(GL.SHADING_LANGUAGE_VERSION));
//     Console.WriteLine( "GL version: " + GL.GetString(GL.VERSION));
//     Console.WriteLine( "Chipset vendor: " + GL.GetString(GL.VENDOR));
//      //gcd.Console.WriteLine(info("Extensions" &  GL.GetString(GL.EXTENSIONS))
//     Initialized = true;

// }
 // Dibuja un rectangulo, relleno o vacio.
 // Puede dibujar un contorno del mismo de otro color->Bounding
 // Mode: 0=relleno, 1=relleno y recuadro, 2=solo recuadro

public static void Rectangle2D(double x1, double y1, double w, double h, int colour1= Colors.Blue, int colour2= -14, int colour3= -14, int colour4= -14, int BoundingColor= Colors.Blue, int BoundingWIdth= 1, double[] Dashes = null, int mode= 0)
    {


    int c2 ;         
    int c3 ;         
    int c4 ;         
    double[] flxVertex ;
        // Quad esta obsoleto , reemplazo por dos triangulos
        if (mode == 0 || mode == 1)
        {

            if (colour2 == -14)
            {
                c2 = colour1 ;
            } else {
                 c2 = colour2;
                 }
        if ( colour3 == -14 )
        {
             c3 = colour1;
              } else { 
                c3 = colour3;
                }
            if (colour4 == -14){
                 c4 = colour1;
        } else         {
            c4 = colour4;
        }
        
        VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Triangles);

        Vertex2D(x1, y1, colour1);
        Vertex2D(x1 + w, y1, c2);
        Vertex2D(x1 + w, y1 + h, c3);

        Vertex2D(x1, y1, colour1);
        Vertex2D(x1 + w, y1 + h, c4);
        Vertex2D(x1, y1 + h, c3);

        

    }

    if ( mode >= 1 ) // solo recuadro
    {

        PolyLines([x1, y1, x1 + w, y1, x1 + w, y1 + h, x1, y1 + h, x1, y1], BoundingColor, BoundingWIdth, Dashes);

    }

}

 // Dibuja un rombo, relleno o vacio.
 // Puede dibujar un contorno del mismo de otro color->Bounding
 // Mode: 0=relleno, 1=relleno y recuadro, 2=solo recuadro
public static void Rombo2D(double x1, double y1, double side, int ColorLeft= Colors.Blue, int ColorRigth= Colors.Blue, int iDirection= 0, int BoundingColor= Colors.Blue, int BoundingWIdth= 1, int mode= 0, double Rotation= 0)
    {


     double[] flxVertex ;         
     // Quad esta obsoleto , reemplazo por dos triangulos
    side /= 2;
    if ( mode == 0 || mode == 1 )
    {

        VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Triangles);

        Vertex2D(x1 - side, y1, ColorLeft);
        Vertex2D(x1, y1 + side, ColorLeft);
        Vertex2D(x1, y1 - side, ColorLeft);

        Vertex2D(x1 + side, y1, ColorRigth);
        Vertex2D(x1, y1 + side, ColorRigth);
        Vertex2D(x1, y1 - side, ColorRigth);

        

    }

    if ( mode >= 1 ) // solo recuadro
    {

        PolyLines([x1 - side, y1, x1, y1 + side, x1 + side, y1, x1, y1 - side, x1 - side, y1], BoundingColor);

    }

}

 // Dibuja un serie de lineas
 public static void DrawLines(double[] fVertices,  int colour = 0, double LineWidth = 1, double[] dashes = null)
    {

    int i ;         
    float r ;         
    float g ;         
    float b ;         
    double[] vertices ;         

     //If gbcolor > 0 Then Stop
    r = (Gb.Shr(colour, 16) & 255) / 256;
    g = (Gb.Shr(colour, 8) & 255) / 256;
    b = (colour & 255) / 256;

    if ( fVertices.Length  < 2 ) return;

    VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Lines);
        for ( i = 0; i < fVertices.Length ; i += 2)
        {
            VboManager.CurrentVBO.AppendVertices([fVertices[i], fVertices[i + 1], 0]); //X, Y, Z
           

          VboManager.CurrentVBO.AppendColors([r,g,b,1]);

          
        }

    }



//  // Dibuja un seria de polilinea
//  public static void PolygonFilled(double[] vertices,  int colour = 0, int FillColor = 0, double LineWidth = 1, double[] dashes = null)
//     {

//     int i ;         

//      //glColorRGB(colour)
    

  

//     GL.Begin(GL.POLYGON);

//     for ( i = 0; i <= vertices.Length  -1; i += 2)
//     {
//         glColorRGB(colour);
//         Vertex2D(vertices[i], vertices[i + 1]);

//     }
    

// }

 // Dibuja un seria de polilinea
 public static void Polygon(double[] vertices,  int colour = 0, double LineWidth = 1, double[] dashes = null)
    {

    int i ;         

     //glColorRGB(colour)
    

    VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.LineStrip);

    for ( i = 0; i <= vertices.Length-1; i+=2)
    {
        glColorRGB(colour);
        Vertex2D(vertices[i], vertices[i + 1]);

    }
    Vertex2D(vertices[0], vertices[ 1]);
    

}
 // Dibuja un arco, suponiendo que el centro esta en 0,0 (despues de un Translate())
 // Siempre gira en sentido anti-horario
 // Las medidas de los angulo inicial y recorrido estan en RADIANES


 public static double[] ArcPoly(double xCenter, double yCenter, double radio, double start_angle, double length,  double angle_increment = Math.PI * 2 / 360)
{
    double theta ;         
    double x0 ;         
    double y0 ;         
    List<double> flxPoly ;         
    int i =0 ;         
     //double max_angle = 2 * Math.PI
     //double angle_increment = Math.PI / 1000
     //angle_increment = Pi * 2 / 360

     // verif=0ico los angulos
     // If length < 0 Then angle_increment *= -1
    flxPoly = new List<double>((int)(length / angle_increment) * 4);
    for ( theta = angle_increment; theta <= length; theta += angle_increment)
    {
        x0 = radio * Math.Cos(start_angle + theta - angle_increment);
        y0 = radio * Math.Sin(start_angle + theta - angle_increment);
        flxPoly[i] = x0 + xCenter;
        i++;
        flxPoly[i] = y0 + yCenter;
        i++;
        x0 = radio * Math.Cos(start_angle + theta);
        y0 = radio * Math.Sin(start_angle + theta);
        flxPoly[i] = x0 + xCenter;
        i++;
        flxPoly[i] = y0 + yCenter;
        i++;

    }
    x0 = radio * Math.Cos(start_angle + theta - angle_increment);
    y0 = radio * Math.Sin(start_angle + theta - angle_increment);

    if ( flxPoly.Count == i )
    {
        flxPoly.Add(0);
        flxPoly.Add(0);
    }
    flxPoly[i] = x0 + xCenter;
    i++;
    flxPoly[i] = y0 + yCenter;
    i++;

    x0 = radio * Math.Cos(start_angle + length);
    y0 = radio * Math.Sin(start_angle + length);

    if ( flxPoly.Count == i )
    {
        flxPoly.Add(0);
        flxPoly.Add(0);
    }
    flxPoly[i] = x0 + xCenter;
    i++;
    flxPoly[i] = y0 + yCenter;
    i++;
     //If flxPoly.Count <> i Then Stop

    return flxPoly.ToArray();

}

 public static void PolyLines(double[] fVertices,  int colour = 0, double LineWidth = 1, double[] dashes = null)
{
    int i ;         
    double[] vertices ;         
    double[] vertices2 ;         
   
    if ( fVertices.Length < 2 ) return;
    

    if ( dashes.Length > 0 )
    {
        vertices = Puntos.DashedLineStrip(fVertices, dashes, 1);

        VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Lines);

 
        for ( i = 0; i <= vertices.Length  -1; i += 2)
        {
             //    glColorRGB(colour)
            Vertex2D(vertices[i], vertices[i + 1],colour);
        }

    } else {

        vertices = fVertices;
        VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.LineStrip);
   
        for ( i = 0; i <= vertices.Length  -1; i += 2)
        {
             //glColorRGB(colour)
            Vertex2D(vertices[i], vertices[i + 1],colour);
        }

    }

    

}

 public static void DrawTriangles(double[] vertices,  int colour = 0, int FillColor = 0, double LineWidth = 1, double[] dashes = null)
{
    int i ;         

     //glColorRGB(colour)
    

    if ( dashes != null && dashes.Length > 0 )
    {

        PolyLines(vertices, colour, LineWidth, dashes);

    }

    VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Triangles);

    for ( i = 0; i <= vertices.Length  -1; i += 2)
    {
        glColorRGB(colour);
        Vertex3F(vertices[i], vertices[i + 1], zLevel);

    }
    

}

 public static void DrawTriangles3D(double[] vertices3D, int[] faces,  int colour = 0, int FillColor = 0, double LineWidth = 1)
{
    int i ;         

     //glColorRGB(colour)
    

    VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Triangles);

    for ( i = 0; i <= faces.Length  -1; i += 3)
    {

        glColorRGB(colour);
        Vertex3F(vertices3D[faces[i]], vertices3D[faces[i + 1]], vertices3D[faces[i + 2]]);

    }
    

}

 // Dibuja un circulo

public static void CIRCLE(double[] center, double radious, int colour= 0, bool Filled= false, double LineWidth= 1, double[] dashes= null)
    {


    double x ;         
    double y ;         
    double theta ;         
    double angle_increment ;         
    int StepFactor = 2;
     List<double> vertices ;         
     List<double> fVertices = new List<double>();         

    

    if ( Filled )
    {
        VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.LineStrip);
        angle_increment = Math.PI * 2 / 360; // esto va de a un grado, que puede ser exagerado

        angle_increment *= StepFactor;
        for ( theta = 0; theta <= 2 * Math.PI; theta += angle_increment)
        {
             // el punto considerando 0,0 al centro
            x = center[0] + radious * Math.Cos(theta);
            y = center[1] + radious * Math.Sin(theta);
            Vertex2D(x, y, colour);
        }
        
    } else {

        angle_increment = Math.PI * 2 / 360; // esto va de a un grado, que puede ser exagerado

        angle_increment *= StepFactor;

        for ( theta = 0; theta <= 2 * Math.PI; theta += angle_increment)
        {
             // el punto considerando 0,0 al centro
            x = center[0] + radious * Math.Cos(theta);
            y = center[1] + radious * Math.Sin(theta);
            fVertices.Add(x);
            fVertices.Add(y);
        }

        PolyLines(fVertices.ToArray(), colour, LineWidth, dashes);

    }

}

 public static void glColorRGB(int gbColor,  double alpha = 1.0)
     // set the color to GL
{
    float r ;         
    float g ;         
    float b ;         
    float a ;         

    a = (float)alpha; // 1 - Colors.GetAlpha(gbColor) / 255
    r = (Gb.Shr(gbColor, 16) & 255) / 255;
    g = (Gb.Shr(gbColor, 8) & 255) / 255;
    b = (gbColor & 255) / 255;
    CurrentColor[0] = r;
    CurrentColor[1] = g;
    CurrentColor[2] = b;
    CurrentColor[3] = a;

}

//  public static double[] GetColorRGBA(int gbColor)
//      // set the color to GL
// {
//     double r ;         
//     double g ;         
//     double b ;         
//     double a ;         

//     a = 1 - Colors.GetAlpha(gbColor) / 255;
//     r = (Gb.Shr(gbColor, 16) & 255) / 255;
//     g = (Shr(gbColor, 8) & 255) / 255;
//     b = (Gb.gbColor & 255) / 255;

//     return [r, g, b, a];

// }

// public static void ClearColor(int iColor)
//     {


//     double[] rgba ;         

//     rgba = GetColorRGBA(iColor);
//     GL.ClearColor(rgba[0], rgba[1], rgba[2], rgba[3]);

// }
 // Define un ColorMaterial: rgba ambient , rgba diffuse, rgba specular, shininess (13 valores en total)

// public static void glMaterial(double[] fMaterial)
//     {


//     GL.Materialfv(GL.FRONT_AND_BACK, GL.AMBIENT, fMaterial.Copy(0, 4));
//     GL.Materialfv(GL.FRONT_AND_BACK, GL.DIFFUSE, fMaterial.Copy(4, 4));
//     GL.Materialfv(GL.FRONT_AND_BACK, GL.SPECULAR, fMaterial.Copy(8, 4));
//     GL.Materiali(GL.FRONT_AND_BACK, GL.SHININESS, CInt(fMaterial[12]));

// }

// public static void glMaterialHierro(double Alpha= 0)
//     {

//      // set the color to GL

//     double r ;         
//     double g ;         
//     double b ;         
//      double[] MyColor ;         

//     MyColor = [0.05375, 0.05, 0.06625, 1.0];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.AMBIENT, MyColor);

//     MyColor = [0.18275, 0.17, 0.22525];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.DIFFUSE, MyColor);

//     MyColor = [0.332741, 0.328634, 0.346435];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.SPECULAR, MyColor);

//     GL.Materialf(GL.FRONT_AND_BACK, GL.SHININESS, 0.3 * 128);

// }

// public static void glMaterialMadera(double Alpha= 0)
//     {

//      // set the color to GL

//     double r ;         
//     double g ;         
//     double b ;         
//      double[] MyColor ;         

//     MyColor = [0.05, 0.05, 0.0, 1.0];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.AMBIENT, MyColor);

//     MyColor = [0.5, 0.5, 0.4];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.DIFFUSE, MyColor);

//     MyColor = [0.7, 0.7, 0.04];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.SPECULAR, MyColor);

//     GL.Materialf(GL.FRONT_AND_BACK, GL.SHININESS, 0.07815 * 128);

// }

// public static void glMaterialConcreto(double Alpha= 0)
//     {

//      // set the color to GL

//      double[] MyColor ;         

//     MyColor = [0.2, 0.2, 0.2, 1.0];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.AMBIENT, MyColor);

//     MyColor = [0.6, 0.6, 0.6];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.DIFFUSE, MyColor);

//     MyColor = [1, 1, 1];

//     GL.Materialfv(GL.FRONT_AND_BACK, GL.SPECULAR, MyColor);

//     GL.Materialf(GL.FRONT_AND_BACK, GL.SHININESS, 64);

// }

 public static void Vertex2D(double x2d, double y2d,  int colour = -1)
     //
     //
     //     //2020 el color va primero
     // //
{
    

    
if (colour != -1) 
{
            glColorRGB(colour);
} else {
     VboManager.CurrentVBO.AppendColors(CurrentColor);

}

    VboManager.CurrentVBO.AppendVertices(new double [] { (double )x2d, (double )y2d , 0.0f});
     //

}

public static void Vertex3D(Punto3d p,  int colour = -1)
     //
     //
     //     //2020 el color va primero
     //
{
   if (colour != -1) 
        {
                glColorRGB(colour);
        } else {
            VboManager.CurrentVBO.AppendColors(CurrentColor);

        }

    VboManager.CurrentVBO.AppendVertices(new double [] { (double )p.x, (double )p.y, (double )p.z});
     //

}

public static void Vertex3F(double X, double Y, double Z,  int colour = -1)
     //
     //
     //     //2020 el color va primero
     //
{
   if (colour != -1) 
        {
                glColorRGB(colour);
        } else {
            VboManager.CurrentVBO.AppendColors(CurrentColor);

        }

    VboManager.CurrentVBO.AppendVertices(new double [] { (double )X, (double )Y, (double )Z});
     //

}



// public static void Get2DpointFrom3Dworld(Punto3d p1, ref double  x2, ref double  y2, ref double  z2)
//     {


//      double[16] modelmatrix ;         
//      double[16] projMatrix ;         
//      double[16] miMatrix ;         

//      int[4] vp ;         

//      double[3] p2 ;         

//     modelMatrix = GL.Getdoublev(GL.MODELVIEW_MATRIX);
//     projMatrix = GL.Getdoublev(GL.PROJECTION_MATRIX);
//     vp = GL.Getintv(GL.VIEWPORT_);

//     p2 = glu.Project(p1.x, p1.y, p1.z, modelMatrix, projMatrix, vp);

//     if ( IsNull(p2) ) return;

//     x2 = p2[0];
//     y2 = p2[1];

//      // z>0 ---> el punto es visible
//     z2 = p2[2];

// }

// public static void Get2DpointFrom3Dworld2(double x, double y, double z, ref double  x2, ref double  y2, ref double  z2)
//     {


//      double[16] modelmatrix ;         
//      double[16] projMatrix ;         
//      double[16] miMatrix ;         

//      int[4] vp ;         

//      double[3] p2 ;         

//     modelMatrix = GL.Getdoublev(GL.MODELVIEW_MATRIX);
//     projMatrix = GL.Getdoublev(GL.PROJECTION_MATRIX);
//     vp = GL.Getintv(GL.VIEWPORT_);

//     p2 = glu.Project(x, y, z, modelMatrix, projMatrix, vp);

//     if ( IsNull(p2) ) return;

//     x2 = p2[0];
//     y2 = p2[1];

//      // z>0 ---> el punto es visible
//     z2 = p2[2];

// }

//  // Proveo las matrices de trnasformacion porque en cada cambio deben guardarse para que esta funcion se corresponda con lo que se muestra
// public static void Get2DpointFrom3Dworld3(double x, double y, double z, double[] ModelMatrix, double[] projMatrix, int[] iViewPort, ref double x2, ref double y2, ref double z2)
//     {


//      double[3] p2 ;         

//     p2 = glu.Project(x, y, z, modelMatrix, projMatrix, iViewPort);

//     if ( IsNull(p2) ) return;

//     x2 = p2[0];
//     y2 = p2[1];

//      // z>0 ---> el punto es visible
//     z2 = p2[2];

// }
//  // Trnasforma un punto de la pantalla en un punto del espacio, que en realidad es un rayo en 3D
//  // que en 2D es perpendicular a la pantalla

// public static double[] Get3DpointFromScreen(int Xscreen, int Yscreen)
//     {


//      double[16] modelmatrix ;         
//      double[16] projMatrix ;         
//      double[16] miMatrix ;         

//      int[4] vp ;         

//      double[3] p2 ;         
//     modelMatrix = GL.Getdoublev(GL.MODELVIEW_MATRIX);
//     projMatrix = GL.Getdoublev(GL.PROJECTION_MATRIX);
//     vp = GL.Getintv(GL.VIEWPORT_);

//     p2 = glu.UnProject(Xscreen, Yscreen, 0, modelMatrix, projMatrix, vp);

//     if ( IsNull(p2) ) return;

//      // p2[2]
//      // z<=1  --> el punto esta frente a la camara
//      // z> 1  ---> el punto es invisible

//      // test
//      //Console.WriteLine( "Screen", Xscreen, Yscreen, " -> Real ", p2[0], p2[1]

//     return p2;

// }

 // Establece la fuente con que se dibujaran los textos
public static bool SelectFont(string FontName)
    {


    if ( glFont.ContainsKey(FontName) )
    {
        ActualFont = glFont[FontName];
        return true; // fuente encontrada

    }

    return false; // fuente no encontrada

}

 // Lee todas las fuentes del directorio provisto y devuelve un listado con sus nombres
public static List<string> LoadFonts(string DirPath)
    {

     // This class loads all fonts available at startup
     // LibreCAD format fonts come in this fashion

     // #Format: LibreCAD Font 1
     // #Creator: LibreCAD
     // #Version: master
     // #Name: Roman Complex
     // #Encoding: UTF - 8
     // #LetterSpacing: 3
     // #WordSpacing: 6.75
     // #LineSpacingFactor: 1
     // #Author: Hershey fonts
     // #Author: Adam Radlowski < adamrinformatyka.gdansk.pl > (Polish)
     // #License: public static domain
     //
     // [0021]!
     // 0.428572, 9; 0, 8.14286
     // 0, 8.14286; 0.428572, 2.99999
     // 0.428572, 2.99999; 0.857143, 8.14286
     // 0.857143, 8.14286; 0.428572, 9
     // 0.428572, 8.14286; 0.428572, 5.57144
     // 0.428572, 0.857143; 0, 0.428572
     // 0, 0.428572; 0.428572, 0
     // 0.428572, 0; 0.857143, 0.428572
     // 0.857143, 0.428572; 0.428572, 0.857143
     //
     // [0022]""
     // 0.428572, 6.00001; 0, 5.57144
     // 0, 5.571

     // - Is there any documentation about the LFF file format?
     // I coped this from a mail:
     //
     // Attached a proposal of new fonts, and below a explanation:
     //
     // [0041] A
     // 0.0000,0.0000;3.0000,9.0000;6.0000,9.000          <---CADA LINEA ES UNA POLY INDEPENDIENTE DE LA ANTERIOR
     // 1.0800,2.5500;4.7300,2.5500
     //
     // line 1 => utf-8 code + letter (same as QCAD)
     // line 2 & 3 =>sequence like polyline vertex with ");" seperating vertex
     // and "," separating x,y coords
     //
     // [0066] f
     // 1.2873,0;1.2873,7.2945;A0.5590,3.4327,9.0000
     // 0.000000,6.0000,3.0000,6.0000
     //
     // line 2  =>sequence like polyline vertex with ");" seperating vertex and
     // "," separating x,y coords, if vertex is prefixed with "A"
     // the first field is a bulge
     //
     // [00C1] Á
     // C0041
     // 2.000000,9.0000,4.0000,10.0000

    // string sFilename ;         
    // File fFile ;         
    string sData ;         
    // string sCoord ;         
    // string aVert ;         
    string sCode ;         
    string[] sPuntos ;         
     List<string> Lista = new List<string>()  ;         
    int p1 ;         
    bool BulgeAdded ;         
    string[] sVert ;         
    // List<double> fltb = new List<double>()  ;         
    // List<double> flt1 ;         
    List<double> flt3 ;         
    // double flt2 ;         

    foreach ( var sFilename in Gb.DirFullPath(DirPath, "*.lff"))
    {

         LFFFonts fntNuevas = new LFFFonts();         

        fntNuevas.FileName = sFilename;
        fntNuevas.WordSpacing = 6.75;
        fntNuevas.LetterSpacing = 1; //.25
        fntNuevas.LineSpacingFactor = 1;
        fntNuevas.Letter = new Dictionary<int, Letters>();
        using var ffile = new StreamReader(sFilename);
        
        while ( !ffile.EndOfStream )
        {
            string? line = ffile.ReadLine();
            if (line == null) break;
            sData = line;
             //Console.WriteLine( sData);
            if ( Gb.Left(sData, 1) == "[" ) // nueva letra
            {
                var letra = new Letters();
                sCode = Gb.Replace(sData, "#", "");
                sCode = Gb.Replace(sCode, "[[", "[");
                sCode = Gb.Mid(sCode, 2, 4);
                letra.Code = Gb.CInt("0x0000" + sCode); // [0021]!
                letra.FontGlyps = new List<List<double>>();
                letra.FontBulges = new List<List<double>>();

                while ( sData != "")
                {
                    sData = ffile.ReadLine() ?? "";
                     //Console.WriteLine( sData);
                    // Input #fFile, sData; // 0.428572, 0.857143; 0, 0.428572
                    if ( Gb.Left(sData, 1) == "C" ) // Copio datos de otra letra
                    {
                        int CopyCode = 0;
                        CopyCode = Gb.CInt("0x0000" + Gb.Mid(sData, 2, 4)); // C0021
                         //CopyCode = GetCodeIndex(fntNuevas, CopyCode)
                        // flt1 = new List<double>();
                        flt3 = [];

                        foreach ( var flt1 in fntNuevas.Letter[CopyCode].FontGlyps)
                        {
                            letra.FontGlyps.Add(flt3);
                            foreach ( var flt2 in flt1)
                            {
                                flt3.Add(flt2);
                            }
                        }
                        flt3 =  [];
                        foreach ( var flt1 in fntNuevas.Letter[CopyCode].FontBulges)
                        {
                            letra.FontBulges.Add(flt3);
                            foreach ( var flt2 in flt1)
                            {
                                flt3.Add(flt2);
                            }
                        }

                    } else {
                        var flt1 = new List<double>();
                        var fltb =  new List<double>();

                        letra.FontGlyps.Add(flt1);
                        letra.FontBulges.Add(fltb);

                        sPuntos = Gb.Split(sData, ");");

                        foreach ( var sCoord in sPuntos)
                        {
                            sVert = Gb.Split(sCoord, ",");
                            BulgeAdded = false;
                            foreach ( var aVert in sVert)
                            {
                                p1 = Gb.InStr(aVert, "A");
                                if ( p1 > 0 )
                                {

                                     // Try letra.FontBulges.Add(Cdouble(Mid$(aVert, p1 + 1)))
                                    fltb.Add(Gb.CDbl(Gb.Mid(aVert, p1 + 1)));
                                    BulgeAdded = true;

                                } else {

                                     // Try letra.FontGlyps.Add(Cdouble(aVert))
                                    flt1.Add(Gb.CDbl(aVert));

                                }
                            }
                            if ( ! BulgeAdded ) fltb.Add(0);
                        }
                    }
                } // fin de la letra
                fntNuevas.Letter.Add(letra.Code, letra);

            } // ignoro todos los comentarios

        }
        fntNuevas.FontName = Gb.FileWithoutExtension(sFilename);
        glFont.Add(fntNuevas.FontName, fntNuevas);
        ActualFont = fntNuevas;
        if ( fntNuevas.FontName == "unicode" ) UnicodeFont = fntNuevas;

        Console.WriteLine( "LeIdas " + fntNuevas.Letter.Count + " letras en " + sFilename);
        Lista.Add(fntNuevas.FontName);

    }

    return Lista;
}

 // // Busca el codigo UTF y devuelve la posicion en el indice de letras
 // public static Function GetCodeIndex(LaFont As FontSt, UTFcode As int) As int
 //
 //   Dim i As int
 //
 //   If Not LaFont Then Return 0
 //
 //   For i = 0 To LaFont.Letter.Length  -1
 //     If LaFont.Letter[i].Code = UTFcode Then Return i
 //   Next
 //
 //   // si estamos aca es porque no lo encontramos, buscamos en unicode
 //   For i = 0 To UnicodeFont.Letter.Length  -1
 //     If UnicodeFont.Letter[i].Code = UTFcode Then Return -i
 //   Next
 //
 // End

 // Grafica un texto en el contexto actual de acuerdo a los parametros pasados
 // Debe estar definida la Font con nombre y altura
 //
 //Fast 


 // Devuelve una poly con el texto en el contexto actual de acuerdo a los parametros pasados
 // Debe estar definida la Font con nombre y altura
 public static double[] DrawTextPoly(string UTFstring, double textH = 1, double sRotationRad = 0, double sItalicAngle = 0, double fScaleX = 1)
{
    int i =0;         
    int iii =0;         
    int i2 =0;         
    int UTFcode =0;         
    int LetterIndex =0;         
    double Xadvance=0 ;         
    double xMax =0 ;
        double[] fArcParams ;
        // double[] Glyps ;         
     List<double> Bulges ;         
   List<List<double>> TGlyps ;         
    List<List<double>> TBulges ;         
    double Ang =0;         
    double m1 =0;         
    double m2 =0;         
    double B =0;         
    double bx = 0 ;         
    double by = 0 ;         
    double mx =0;         
    double my=0 ;         
    double ang1 =0;         
    double Lt =0;         
    int iBulge =0;         

    double dX =0;          // donde tengo el cursor
    double dY =0;         
    double alpha =0;         
     double[] flxArc    = new double[0];         
     double[] flxGlyps = new double[0];         
     double[] flxAnswer = new double[0];    


     //SelectFont("romant")
     // GL.Scalef(textH * FontScale, textH * FontScale, 1)
    for ( i = 1; i <= UTFstring.Length ; i++ ) // para cada letra
    {
        UTFcode = Gb.GetUnicodeCodePoint(UTFstring, i); // obtengo el UTF code

        if ( UTFcode == 32 ) // es un espacio
        {
            Xadvance += ActualFont.WordSpacing; // muevo el puntero a la siguiente posicion

        } else {
             // DEPRE LetterIndex = GetCodeIndex(ActualFont, UTFcode)                     // obtengo el indice de la letra
            if ( ActualFont.Letter.ContainsKey(UTFcode) )
            {
                TGlyps = ActualFont.Letter[UTFcode].FontGlyps;
                TBulges = ActualFont.Letter[UTFcode].FontBulges;

            } // is unicode
            else if ( UnicodeFont.Letter.ContainsKey(UTFcode) )
            {
                TGlyps = UnicodeFont.Letter[UTFcode].FontGlyps;
                TBulges = UnicodeFont.Letter[UTFcode].FontBulges;

            } else {
                 // descarto la letra
                continue;

            }

             // con bulges
             //============================================================================
            iBulge = 0;

            foreach ( var Glyps in TGlyps)
            {
                Bulges = TBulges[iBulge];

                for ( i2 = 0; i2 <= Glyps.Count / 2 - 2; i2 += 1)
                {

                     // no todos los tramos pueden tener bulges
                    if ( Gb.Abs(Bulges[i2 + 1]) > 0.001 )
                    {
                         // // FIXME: arc problem
                         // Continue
                        ang1 = Gb.Ang(Glyps[(i2 + 1) * 2 + 1] - Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2] - Glyps[i2 * 2]); // angulo del tramo
                        Lt = Puntos.distancia(Glyps[i2 * 2], Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);
                        if ( Lt == 0 ) continue;
                        mx = (Glyps[(i2 + 1) * 2] + Glyps[i2 * 2]) / 2; // punto medio del tramo
                        my = (Glyps[(i2 + 1) * 2 + 1] + Glyps[i2 * 2 + 1]) / 2;
                        B = Bulges[i2 + 1] * Lt / 2;
                        if ( Bulges[i2 + 1] < 0 ) {alpha = Math.PI / 2; } else { alpha = -Math.PI / 2; }
                        bx = mx + B * Math.Cos(ang1 + alpha); // Tercer punto del Bulge
                        by = my + B * Math.Sin(ang1 + alpha);

                         // aqui podria usar una rutina de arco entre 3 puntos

                        fArcParams = Puntos.Arc3Point(Glyps[i2 * 2], Glyps[i2 * 2 + 1], bx, by, Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);

                        flxArc = ArcPoly(fArcParams[0] + Xadvance, fArcParams[1], fArcParams[2], fArcParams[3], fArcParams[4], Math.PI / 16);

                        flxAnswer = Gb.AppendArray(flxAnswer, flxArc);

                        Array.Clear(fArcParams);
                        Array.Clear(flxArc);

                    } else { // dibujo la linea normalmente

                        flxAnswer = Gb.AppendArray(flxAnswer, new double[] { Glyps[i2 * 2] + Xadvance, Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2] + Xadvance, Glyps[(i2 + 1) * 2 + 1] });
                    }

                }
                for ( iii = 0; iii <= Glyps.Count  -1; iii += 2) // calculo cuanto tiene que avanzar el puntero
                {
                    if ( Glyps[iii] > xMax ) xMax = Glyps[iii];
                }

                iBulge++;
            }
            Xadvance += xMax + ActualFont.LetterSpacing; // muevo el puntero a la siguiente posicion

            xMax = 0;
             //================================================================================

        }
    }

    Puntos.Scale(flxAnswer, textH * FontScale * fScaleX, textH * FontScale);

    if ( sItalicAngle != 0 )
    {

        for ( iii = 0; iii <= flxAnswer.Length  -1 - 1; iii += 2)
        {
            flxAnswer[iii] += flxAnswer[iii + 1] *Math.Sin(Gb.DegreesToRadians(sItalicAngle));
        }

    }
    if ( sRotationRad != 0 ) Puntos.Rotate(flxAnswer, sRotationRad);
    return flxAnswer;

}

//  // AlingHoriz : 0=Rigth, 1=Center, 2=Left
//  // AlingVert: 0=Top, 1=Center, 2=Bottom
// public static bool DrawText2(string UTFstring, double posX, double posY, double angle= 0, double textH= 1, int colour= -14, int Backcolour= -1, double LineWidth= 1, bool italic= false, double rectW= 0, double rectH= 0, int alignHoriz= 0, int alignVert= 0)
//     {


//     double[] flxText ;         
//     double[] tRect ;         
//     double sItalicAngle ;         
//     double tX ;         
//     double tY ;         
//     double factorX ;         
//     double factorY ;         
//     double fBorderExtension = 3;

//     if ( italic ) sItalicAngle = 20;

//     flxText = DrawTextPoly(UTFstring, textH, angle, sItalicAngle);
//     tRect = Puntos.Limits(flxText);

//      // veo si tengo que comprimir en un ractangulo
//     if ( (rectH > 0) & (rectW > 0) )
//     {

//         factorX = rectW / (tRect[2] - tRect[0]);
//         factorY = rectH / (tRect[3] - tRect[1]);

//         Puntos.Scale(flxText, factorX, factorY);

//     } else {

//         rectH = tRect[3] - tRect[1];
//         rectW = tRect[2] - tRect[0];
//     }

//     if ( alignHoriz == 1 ) tX = -rectW / 2;
//     if ( alignHoriz == 2 ) tX = -rectW;

//     if ( alignVert == 1 ) tY = -rectH / 2;
//     if ( alignVert == 2 ) tY = -rectH;

//     Puntos.Translate(flxText, tx, ty);

//     GL.MatrixMode(GL.PROJECTION);
//     GL.PushMatrix;

//     GL.LoadIdentity();

//     GL.Ortho(0, fmain.gestru.w, 0, fmain.gestru.h, 0, 1);

//     GL.MatrixMode(GL.MODELVIEW);
//     GL.PushMatrix;
//     GL.LoadIdentity();
//     GL.Translatef(posX, posY, 0);
//     DrawLines(flxText, colour, LineWidth);
//     Rectangle2D(tx - fBorderExtension, ty - fBorderExtension, rectW + fBorderExtension * 2, rectH + fBorderExtension * 2, Backcolour,,,,,,, 0);

//     GL.PopMatrix;
//     GL.MatrixMode(GL.PROJECTION);
//     GL.PopMatrix;
//     return true;

// }

//  // AlingHoriz : 0=Rigth, 1=Center, 2=Left
//  // AlingVert: 0=Top, 1=Center, 2=Bottom
// public static bool DrawText3(string UTFstring, double posX, double posY, double posZ, double angle= 0, double textH= 1, int colour= -14, int Backcolour= -1, double LineWidth= 1, bool italic= false, double rectW= 0, double rectH= 0, int alignHoriz= 0, int alignVert= 0)
//     {


//      double[] flxText ;         
//      double[] tRect ;         
//     double sItalicAngle ;         
//     double tX ;         
//     double tY ;         
//     double factorX ;         
//     double factorY ;         
//     double fBorderExtension = 3;

//     if ( italic ) sItalicAngle = 20;

//     flxText = DrawTextPoly(UTFstring, textH, angle, sItalicAngle);
//     tRect = Puntos.Limits(flxText);

//      // veo si tengo que comprimir en un ractangulo
//     if ( (rectH > 0) & (rectW > 0) )
//     {

//         factorX = rectW / (tRect[2] - tRect[0]);
//         factorY = rectH / (tRect[3] - tRect[1]);

//         Puntos.Scale(flxText, factorX, factorY);

//     } else {

//         rectH = tRect[3] - tRect[1];
//         rectW = tRect[2] - tRect[0];
//     }

//     if ( alignHoriz == 1 ) tX = -rectW / 2;
//     if ( alignHoriz == 2 ) tX = -rectW;

//     if ( alignVert == 1 ) tY = -rectH / 2;
//     if ( alignVert == 2 ) tY = -rectH;

//     Puntos.Translate(flxText, tx, ty);

//      // GL.MatrixMode(GL.PROJECTION)
//      // GL.PushMatrix
//      //
//      // GL.LoadIdentity()
//      //
//      // GL.Ortho(0, fmain.gestru.w, 0, fmain.gestru.h, 0, 1)
//      //
//      // GL.MatrixMode(GL.MODELVIEW)
//      // GL.PushMatrix
//      // GL.LoadIdentity()
//     GL.Translatef(posX, posY, 0);
//     DrawLines(flxText, colour, LineWidth);
//     Rectangle2D(tx - fBorderExtension, ty - fBorderExtension, rectW + fBorderExtension * 2, rectH + fBorderExtension * 2, Backcolour, Backcolour , Backcolour, Backcolour, Backcolour, 0,0, 0);

//      // GL.PopMatrix
//      // GL.MatrixMode(GL.PROJECTION)
//      // GL.PopMatrix

// }

 // devuelve un rectangulo que contiene al texto
 // [ancho,alto]
 public static double[] TextExtends(string UTFstring, double textH = 1, double sRotationRad = 0, double sItalicAngle = 0)
    {

     double[] flxText ;         
     double[] tRect ;         

    flxText = DrawTextPoly(UTFstring, textH, sRotationRad, sItalicAngle);
    tRect = Puntos.Limits(flxText);
     //tRect[1] *= textH * FontScale * 1.2

    return tRect;

}

//  // Lee todas las texturas del directorio provisto y devuelve un listado con sus nombres
// public static string[] LoadTextures(string DirPath)
//     {


//     string sFilename ;         
//     File fFile ;         
//     string sCoord ;         
//      string[] Lista ;         

//     int iTexture = 0;
//     TextureSt newTexture ;         

//     hText = GL.GenTextures(1);

//     foreach ( var sFilename in Gb.Dir(DirPath, "*.png"))
//     {

//         newTexture = new TextureSt();

//         glTextures.Add(newTexture);

//         newTexture.FileName = Gb.Left(sFilename, -4); // agrego el nombre de la textura a la lista que voy a retornar

//         lista.Add(newTexture.FileName);

//         newTexture.hImage = Image.Load( DirPath + sFilename); // cargo la imagen en memoria

//         GL.TexImage2D(newTexture.hImage); // genero un objeto OpenGL

//         GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MIN_FILTER, GL.NEAREST); // parametros basicos opengl

//         GL.TexParameteri(GL.TEXTURE_2D, GL.TEXTURE_MAG_FILTER, GL.NEAREST); // parametros basicos opengl

//         GL.BindTexture(GL.TEXTURE_2D, hText[iTexture]); // enlazo la textura a una handle

//         newTexture.Id = hText[iTexture];

//         i++Texture;
//         Break;
//     }

//     Console.WriteLine( "LeIdas " + iTexture + " texturas en " + sFilename);

//     return lista;

// }

//  // Dibuja un triangulo con una textura ya cargada

// public static void TexturedTriangle2D(double x1, double y1, double x2, double y2, double x3, double y3, int TextureId, double Scale)
//     {


//     GL.TexImage2D(glTextures[TextureId].hImage); // genero un objeto OpenGL
//     GL.BindTexture(GL.TEXTURE_2D, hText[TextureId]); // enlazo la textura a una handle
//     GL.begin(GL.TRIANGLES);

//     GL.TexCoord2f(0, 0);

//     Vertex2D(x1, y1);

//     GL.TexCoord2f((x2 - x1) / scale / 1000, (y2 - y1) / scale / 1000);

//     Vertex2D(x2, y2);

//     GL.TexCoord2f((x3 - x1) / scale / 1000, (y3 - y1) / scale / 1000);

//     Vertex2D(x3, y3);

    

// }



// public static bool CheckExtension(string sExtension)
//     {


//     return (InStr(LCase(GLx.glGetString(EXTENSIONS)), LCase(sExtension)) > 0);

// }




// public static void DrawText3D(string texto, Punto3d pr, double Altura= 12, long _color= Colors.Blue, long _BackColor= -1, int centradoH= 0, int centradoV= 0)
//     {


//     double x ;         
//     double y ;         
//     double z ;         

//     glx.Get2DpointFrom3Dworld(pr, ByRef x, ByRef y, ByRef z);

//     if ( z <= 1 ) DrawText2(texto, x, y, 0, altura, _color, _backcolor,,,,, centradoH, centradoV);

// }

 // public static Sub LucesOn()
 //
 //     GL.Lightfv(GL.LIGHT0, GL.AMBIENT_AND_DIFFUSE, [1.0, 1.0, 1.0, 0.5])
 //
 //     GL.Lightfv(GL.LIGHT0, GL.POSITION, [GLCam.camera.Position.x, GLCam.camera.Position.y, -GLCam.camera.Position.z, 1])
 //
 //     GL.Enable(GL.LIGHTING)
 //     GL.Enable(GL.LIGHT0)
 //
 //     GL.Enable(GL.NORMALIZE) // esto sirve para normalizar los vectores normales , o sea que sean de largo = 1
 //
 // End
 //
 // public static Sub LucesOff()
 //
 //     GL.Disable(GL.LIGHTING)
 //     GL.disable(GL.LIGHT0)
 //
 //     GL.disable(GL.NORMALIZE) // esto sirve para normalizar los vectores normales , o sea que sean de largo = 1
 //
 // End Sub

public static void GLQuadColor4F(Punto3d p1, Punto3d p2, Punto3d p3, Punto3d p4, int c1, int c2, int c3, int c4)
    {


     // Quad esta obsoleto , reemplazo por dos triangulos

    VboManager.CurrentVBO.SetCurrentPrimitiveType(PrimitiveType.Triangles);

    Vertex3D(p1, c1);
    Vertex3D(p2, c2);
    Vertex3D(p3, c3);
    

    
    Vertex3D(p1, c1);
    Vertex3D(p3, c3);
    Vertex3D(p4, c4);

    

}

}