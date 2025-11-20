using Gaucho;
public static class Glx
{
 // Gambas module file

 // Copyright (C) Ing Martin P Cristia
 //
 // This program is free software; you can redistribute it and/or modify
 // it under the terms of the GNU General Public License as published by
 // the Free Software Foundation; either version 3 of the License, or
 // (at your option) any later version.
 //
 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General Public License for more details.
 //
 // You should have received a copy of the GNU General Public License
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
 //       |                                         |bottom                                           |gldrwArea.w,.h = Gl.Viewport = el area donde dibujo
 //       +-------------------------------------------------------------------------------------------+           = pixeles
 //Fast 
 // una lbreria de funciones para pasar de Paint a OpenGL
public bool Initialized = false;
public LFFFonts ActualFont ;          // the class
public LFFFonts UnicodeFont ;         
public string ActualFontName = "romanc");           // the name
public double ActualFontHeigth = 1;                 // the letter heigth
public double FontScale = 0.1125;                      // the general scale factor

public double zLevel = 0;

public string[] FontsNameList ;          // lista de fuentes LFF disponibles ya cargadas

public int[] FontsCAllLists ;          // listas de listas de caracteres

public  Dictionary<string, string> glFont ;         
public struct TextureSt
{
    public string FileName ;
    public string TextureName ;
    public int Id ;
    public Image hImage ;
}

public  TextureSt[] glTextures ;
public struct GLColorSt
{

    double r ;
    double g ;
    double b ;
    double Alpha ;

}
 // A shader is a small C program that the GPU understands
 // minimal shaders we need to compile at the GPU

public string shaVertexShaderSource ;         
public string shaFragmentShaderSource ;         

public int shaVert1 ;          //  main vertex shader
public int shaFrag1 ;          //  main fragment shader
public int shaderProgram ;          //  el programa que corre en la GPU

public double escalaGL ;         

public int ViewportWIdth ;         
public int ViewportHeight ;         

public  int[] hText ;         

 // new OpenGL stuff
public  int[]                        GLDrwList ;          // all entities, each one
public  int[]                 GLDrwListEditing ;          // all entities on edit by some tool, including new ones

 // lineas de puntos
public  int[] LineStipples ;         
public  int[] LineStippleScales ;         

 // modo inmediato o programado
public bool InmediateMode = true;
public bool InmediateModeRequired ;         
public bool VBO_present = false;    // si tenemos VBO que dibujar

 // VBO
public int VBO_Id ;          //Current VBO buffer
public  float[][] VBO_vertex ;         
public  float[][] VBO_colors ;         
public  float[][] VBO_normals ;         
public  float[][] VBO_pixels ;         
public  int[] VBO_Primitive ;         
 //
 // Copyright (C) Ing Martin P Cristia
 //
 // This program is free software; you can redistribute it and/or modify
 // it under the terms of the GNU General Public License as published by
 // the Free Software Foundation; either version 3 of the License, or
 // (at your option) any later version.
 //
 // This program is distributed in the hope that it will be useful,
 // but WITHOUT ANY WARRANTY; without even the implied warranty of
 // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 // GNU General Public License for more details.
 //
 // You should have received a copy of the GNU General Public License
 // along with this program; if not, write to the Free Software
 // Foundation, Inc., 51 Franklin St, Fifth Floor,
 // Boston, MA  02110-1301  USA


 // WARNING: FLOAT en Gambas = DOUBLE en GL
 //          SINGLE en Gambas = FLOAT en GL

const int DEPTH_BUFFER_BIT = 0x000000000100;
const int STENCIL_BUFFER_BIT = 0x000000000400;
const int COLOR_BUFFER_BIT = 0x000000004000;
const int FALSE = 0;
const int TRUE = 1;
const int POINTS = 0x00000000;
const int LINES = 0x00000001;
const int LINE_LOOP = 0x00000002;
const int LINE_STRIP = 0x00000003;
const int TRIANGLES = 0x00000004;
const int TRIANGLE_STRIP = 0x00000005;
const int TRIANGLE_FAN = 0x00000006;
const int QUADS = 0x00000007;
const int NEVER = 0x00000200;
const int LESS = 0x00000201;
const int EQUAL = 0x00000202;
const int LEQUAL = 0x00000203;
const int GREATER = 0x00000204;
const int NOTEQUAL = 0x00000205;
const int GEQUAL = 0x00000206;
const int ALWAYS = 0x00000207;
const int ZERO = 0;
const int ONE = 1;
const int SRC_COLOR = 0x00000300;
const int ONE_MINUS_SRC_COLOR = 0x00000301;
const int SRC_ALPHA = 0x00000302;
const int ONE_MINUS_SRC_ALPHA = 0x00000303;
const int DST_ALPHA = 0x00000304;
const int ONE_MINUS_DST_ALPHA = 0x00000305;
const int DST_COLOR = 0x00000306;
const int ONE_MINUS_DST_COLOR = 0x00000307;
const int SRC_ALPHA_SATURATE = 0x00000308;
const int NONE = 0;
const int FRONT_LEFT = 0x00000400;
const int FRONT_RIGHT = 0x00000401;
const int BACK_LEFT = 0x00000402;
const int BACK_RIGHT = 0x00000403;
const int FRONT = 0x00000404;
const int BACK = 0x00000405;
const int LEFT = 0x00000406;
const int RIGHT = 0x00000407;
const int FRONT_AND_BACK = 0x00000408;
const int NO_ERROR = 0;
const int INVALID_ENUM = 0x00000500;
const int INVALID_VALUE = 0x00000501;
const int INVALID_OPERATION = 0x00000502;
const int OUT_OF_MEMORY = 0x00000505;
const int CW = 0x00000900;
const int CCW = 0x00000901;
const int POINT_SIZE = 0x00000B11;
const int POINT_SIZE_RANGE = 0x00000B12;
const int POINT_SIZE_GRANULARITY = 0x00000B13;
const int LINE_SMOOTH = 0x00000B20;
const int LINE_WIDTH = 0x00000B21;
const int LINE_WIDTH_RANGE = 0x00000B22;
const int LINE_WIDTH_GRANULARITY = 0x00000B23;
const int POLYGON_MODE = 0x00000B40;
const int POLYGON_SMOOTH = 0x00000B41;
const int CULL_FACE = 0x00000B44;
const int CULL_FACE_MODE = 0x00000B45;
const int FRONT_FACE = 0x00000B46;
const int DEPTH_RANGE = 0x00000B70;
const int DEPTH_TEST = 0x00000B71;
const int DEPTH_WRITEMASK = 0x00000B72;
const int DEPTH_CLEAR_VALUE = 0x00000B73;
const int DEPTH_FUNC = 0x00000B74;
const int STENCIL_TEST = 0x00000B90;
const int STENCIL_CLEAR_VALUE = 0x00000B91;
const int STENCIL_FUNC = 0x00000B92;
const int STENCIL_VALUE_MASK = 0x00000B93;
const int STENCIL_FAIL = 0x00000B94;
const int STENCIL_PASS_DEPTH_FAIL = 0x00000B95;
const int STENCIL_PASS_DEPTH_PASS = 0x00000B96;
const int STENCIL_REF = 0x00000B97;
const int STENCIL_WRITEMASK = 0x00000B98;
const int VIEWPORT = 0x00000BA2;
const int DITHER = 0x00000BD0;
const int BLEND_DST = 0x00000BE0;
const int BLEND_SRC = 0x00000BE1;
const int BLEND = 0x00000BE2;
const int LOGIC_OP_MODE = 0x00000BF0;
const int DRAW_BUFFER = 0x00000C01;
const int READ_BUFFER = 0x00000C02;
const int SCISSOR_BOX = 0x00000C10;
const int SCISSOR_TEST = 0x00000C11;
const int COLOR_CLEAR_VALUE = 0x00000C22;
const int COLOR_WRITEMASK = 0x00000C23;
const int DOUBLEBUFFER = 0x00000C32;
const int STEREO = 0x00000C33;
const int LINE_SMOOTH_HINT = 0x00000C52;
const int POLYGON_SMOOTH_HINT = 0x00000C53;
const int UNPACK_SWAP_BYTES = 0x00000CF0;
const int UNPACK_LSB_FIRST = 0x00000CF1;
const int UNPACK_ROW_LENGTH = 0x00000CF2;
const int UNPACK_SKIP_ROWS = 0x00000CF3;
const int UNPACK_SKIP_PIXELS = 0x00000CF4;
const int UNPACK_ALIGNMENT = 0x00000CF5;
const int PACK_SWAP_BYTES = 0x00000D00;
const int PACK_LSB_FIRST = 0x00000D01;
const int PACK_ROW_LENGTH = 0x00000D02;
const int PACK_SKIP_ROWS = 0x00000D03;
const int PACK_SKIP_PIXELS = 0x00000D04;
const int PACK_ALIGNMENT = 0x00000D05;
const int MAX_TEXTURE_SIZE = 0x00000D33;
const int MAX_VIEWPORT_DIMS = 0x00000D3A;
const int SUBPIXEL_BITS = 0x00000D50;
const int TEXTURE_1D = 0x00000DE0;
const int TEXTURE_2D = 0x00000DE1;
const int TEXTURE_WIDTH = 0x00001000;
const int TEXTURE_HEIGHT = 0x00001001;
const int TEXTURE_BORDER_COLOR = 0x00001004;
const int DONT_CARE = 0x00001100;
const int FASTEST = 0x00001101;
const int NICEST = 0x00001102;
const int BYTE = 0x00001400;
const int UNSIGNED_BYTE = 0x00001401;
const int SHORT = 0x00001402;
const int UNSIGNED_SHORT = 0x00001403;
const int INT = 0x00001404;
const int UNSIGNED_INT = 0x00001405;
const int FLOAT = 0x00001406;
const int STACK_OVERFLOW = 0x00000503;
const int STACK_UNDERFLOW = 0x00000504;
const int CLEAR = 0x00001500;
const int AND = 0x00001501;
const int AND_REVERSE = 0x00001502;
const int COPY = 0x00001503;
const int AND_INVERTED = 0x00001504;
const int NOOP = 0x00001505;
const int XOR = 0x00001506;
const int OR = 0x00001507;
const int NOR = 0x00001508;
const int EQUIV = 0x00001509;
const int INVERT = 0x0000150A;
const int OR_REVERSE = 0x0000150B;
const int COPY_INVERTED = 0x0000150C;
const int OR_INVERTED = 0x0000150D;
const int NAND = 0x0000150E;
const int SET = 0x0000150F;
const int TEXTURE = 0x00001702;
const int COLOR = 0x00001800;
const int DEPTH = 0x00001801;
const int STENCIL = 0x00001802;
const int STENCIL_INDEX = 0x00001901;
const int DEPTH_COMPONENT = 0x00001902;
const int RED = 0x00001903;
const int GREEN = 0x00001904;
const int BLUE = 0x00001905;
const int ALPHA = 0x00001906;
const int RGB = 0x00001907;
const int RGBA = 0x00001908;
const int POINT = 0x00001B00;
const int LINE = 0x00001B01;
const int FILL = 0x00001B02;
const int KEEP = 0x00001E00;
const int REPLACE = 0x00001E01;
const int INCR = 0x00001E02;
const int DECR = 0x00001E03;
const int VENDOR = 0x00001F00;
const int RENDERER = 0x00001F01;
const int VERSION = 0x00001F02;
const int EXTENSIONS = 0x00001F03;
const int NEAREST = 0x00002600;
const int LINEAR = 0x00002601;
const int NEAREST_MIPMAP_NEAREST = 0x00002700;
const int LINEAR_MIPMAP_NEAREST = 0x00002701;
const int NEAREST_MIPMAP_LINEAR = 0x00002702;
const int LINEAR_MIPMAP_LINEAR = 0x00002703;
const int TEXTURE_MAG_FILTER = 0x00002800;
const int TEXTURE_MIN_FILTER = 0x00002801;
const int TEXTURE_WRAP_S = 0x00002802;
const int TEXTURE_WRAP_T = 0x00002803;
const int REPEAT = 0x00002901;

const int COLOR_LOGIC_OP = 0x00000BF2;
const int POLYGON_OFFSET_UNITS = 0x00002A00;
const int POLYGON_OFFSET_POINT = 0x00002A01;
const int POLYGON_OFFSET_LINE = 0x00002A02;
const int POLYGON_OFFSET_FILL = 0x00008037;
const int POLYGON_OFFSET_FACTOR = 0x00008038;
const int TEXTURE_BINDING_1D = 0x00008068;
const int TEXTURE_BINDING_2D = 0x00008069;
const int TEXTURE_INTERNAL_FORMAT = 0x00001003;
const int TEXTURE_RED_SIZE = 0x0000805C;
const int TEXTURE_GREEN_SIZE = 0x0000805D;
const int TEXTURE_BLUE_SIZE = 0x0000805E;
const int TEXTURE_ALPHA_SIZE = 0x0000805F;
const int DOUBLE = 0x0000140A;
const int PROXY_TEXTURE_1D = 0x00008063;
const int PROXY_TEXTURE_2D = 0x00008064;
const int R3_G3_B2 = 0x00002A10;
const int RGB4 = 0x0000804F;
const int RGB5 = 0x00008050;
const int RGB8 = 0x00008051;
const int RGB10 = 0x00008052;
const int RGB12 = 0x00008053;
const int RGB16 = 0x00008054;
const int RGBA2 = 0x00008055;
const int RGBA4 = 0x00008056;
const int RGB5_A1 = 0x00008057;
const int RGBA8 = 0x00008058;
const int RGB10_A2 = 0x00008059;
const int RGBA12 = 0x0000805A;
const int RGBA16 = 0x0000805B;
const int VERTEX_ARRAY = 0x00008074;
const int NORMAL_ARRAY = 0x00008075;
const int COLOR_ARRAY = 0x00008076;
const int INDEX_ARRAY = 0x00008077;
const int TEXTURE_COORD_ARRAY = 0x00008078;
const int EDGE_FLAG_ARRAY = 0x00008079;

const int UNSIGNED_BYTE_3_3_2 = 0x00008032;
const int UNSIGNED_SHORT_4_4_4_4 = 0x00008033;
const int UNSIGNED_SHORT_5_5_5_1 = 0x00008034;
const int UNSIGNED_INT_8_8_8_8 = 0x00008035;
const int UNSIGNED_INT_10_10_10_2 = 0x00008036;
const int TEXTURE_BINDING_3D = 0x0000806A;
const int PACK_SKIP_IMAGES = 0x0000806B;
const int PACK_IMAGE_HEIGHT = 0x0000806C;
const int UNPACK_SKIP_IMAGES = 0x0000806D;
const int UNPACK_IMAGE_HEIGHT = 0x0000806E;
const int TEXTURE_3D = 0x0000806F;
const int PROXY_TEXTURE_3D = 0x00008070;
const int TEXTURE_DEPTH = 0x00008071;
const int TEXTURE_WRAP_R = 0x00008072;
const int MAX_3D_TEXTURE_SIZE = 0x00008073;
const int UNSIGNED_BYTE_2_3_3_REV = 0x00008362;
const int UNSIGNED_SHORT_5_6_5 = 0x00008363;
const int UNSIGNED_SHORT_5_6_5_REV = 0x00008364;
const int UNSIGNED_SHORT_4_4_4_4_REV = 0x00008365;
const int UNSIGNED_SHORT_1_5_5_5_REV = 0x00008366;
const int UNSIGNED_INT_8_8_8_8_REV = 0x00008367;
const int UNSIGNED_INT_2_10_10_10_REV = 0x00008368;
const int BGR = 0x000080E0;
const int BGRA = 0x000080E1;
const int MAX_ELEMENTS_VERTICES = 0x000080E8;
const int MAX_ELEMENTS_INDICES = 0x000080E9;
const int CLAMP_TO_EDGE = 0x0000812F;
const int TEXTURE_MIN_LOD = 0x0000813A;
const int TEXTURE_MAX_LOD = 0x0000813B;
const int TEXTURE_BASE_LEVEL = 0x0000813C;
const int TEXTURE_MAX_LEVEL = 0x0000813D;
const int SMOOTH_POINT_SIZE_RANGE = 0x00000B12;
const int SMOOTH_POINT_SIZE_GRANULARITY = 0x00000B13;
const int SMOOTH_LINE_WIDTH_RANGE = 0x00000B22;
const int SMOOTH_LINE_WIDTH_GRANULARITY = 0x00000B23;
const int ALIASED_LINE_WIDTH_RANGE = 0x0000846E;
const int TEXTURE0 = 0x000084C0;
const int TEXTURE1 = 0x000084C1;
const int TEXTURE2 = 0x000084C2;
const int TEXTURE3 = 0x000084C3;
const int TEXTURE4 = 0x000084C4;
const int TEXTURE5 = 0x000084C5;
const int TEXTURE6 = 0x000084C6;
const int TEXTURE7 = 0x000084C7;
const int TEXTURE8 = 0x000084C8;
const int TEXTURE9 = 0x000084C9;
const int TEXTURE10 = 0x000084CA;
const int TEXTURE11 = 0x000084CB;
const int TEXTURE12 = 0x000084CC;
const int TEXTURE13 = 0x000084CD;
const int TEXTURE14 = 0x000084CE;
const int TEXTURE15 = 0x000084CF;
const int TEXTURE16 = 0x000084D0;
const int TEXTURE17 = 0x000084D1;
const int TEXTURE18 = 0x000084D2;
const int TEXTURE19 = 0x000084D3;
const int TEXTURE20 = 0x000084D4;
const int TEXTURE21 = 0x000084D5;
const int TEXTURE22 = 0x000084D6;
const int TEXTURE23 = 0x000084D7;
const int TEXTURE24 = 0x000084D8;
const int TEXTURE25 = 0x000084D9;
const int TEXTURE26 = 0x000084DA;
const int TEXTURE27 = 0x000084DB;
const int TEXTURE28 = 0x000084DC;
const int TEXTURE29 = 0x000084DD;
const int TEXTURE30 = 0x000084DE;
const int TEXTURE31 = 0x000084DF;
const int ACTIVE_TEXTURE = 0x000084E0;
const int MULTISAMPLE = 0x0000809D;
const int SAMPLE_ALPHA_TO_COVERAGE = 0x0000809E;
const int SAMPLE_ALPHA_TO_ONE = 0x0000809F;
const int SAMPLE_COVERAGE = 0x000080A0;
const int SAMPLE_BUFFERS = 0x000080A8;
const int SAMPLES = 0x000080A9;
const int SAMPLE_COVERAGE_VALUE = 0x000080AA;
const int SAMPLE_COVERAGE_INVERT = 0x000080AB;
const int TEXTURE_CUBE_MAP = 0x00008513;
const int TEXTURE_BINDING_CUBE_MAP = 0x00008514;
const int TEXTURE_CUBE_MAP_POSITIVE_X = 0x00008515;
const int TEXTURE_CUBE_MAP_NEGATIVE_X = 0x00008516;
const int TEXTURE_CUBE_MAP_POSITIVE_Y = 0x00008517;
const int TEXTURE_CUBE_MAP_NEGATIVE_Y = 0x00008518;
const int TEXTURE_CUBE_MAP_POSITIVE_Z = 0x00008519;
const int TEXTURE_CUBE_MAP_NEGATIVE_Z = 0x0000851A;
const int PROXY_TEXTURE_CUBE_MAP = 0x0000851B;
const int MAX_CUBE_MAP_TEXTURE_SIZE = 0x0000851C;
const int COMPRESSED_RGB = 0x000084ED;
const int COMPRESSED_RGBA = 0x000084EE;
const int TEXTURE_COMPRESSION_HINT = 0x000084EF;
const int TEXTURE_COMPRESSED_IMAGE_SIZE = 0x000086A0;
const int TEXTURE_COMPRESSED = 0x000086A1;
const int NUM_COMPRESSED_TEXTURE_FORMATS = 0x000086A2;
const int COMPRESSED_TEXTURE_FORMATS = 0x000086A3;
const int CLAMP_TO_BORDER = 0x0000812D;

const int BLEND_DST_RGB = 0x000080C8;
const int BLEND_SRC_RGB = 0x000080C9;
const int BLEND_DST_ALPHA = 0x000080CA;
const int BLEND_SRC_ALPHA = 0x000080CB;
const int POINT_FADE_THRESHOLD_SIZE = 0x00008128;
const int DEPTH_COMPONENT16 = 0x000081A5;
const int DEPTH_COMPONENT24 = 0x000081A6;
const int DEPTH_COMPONENT32 = 0x000081A7;
const int MIRRORED_REPEAT = 0x00008370;
const int MAX_TEXTURE_LOD_BIAS = 0x000084FD;
const int TEXTURE_LOD_BIAS = 0x00008501;
const int INCR_WRAP = 0x00008507;
const int DECR_WRAP = 0x00008508;
const int TEXTURE_DEPTH_SIZE = 0x0000884A;
const int TEXTURE_COMPARE_MODE = 0x0000884C;
const int TEXTURE_COMPARE_FUNC = 0x0000884D;
const int BLEND_COLOR = 0x00008005;
const int BLEND_EQUATION = 0x00008009;
const int CONSTANT_COLOR = 0x00008001;
const int ONE_MINUS_CONSTANT_COLOR = 0x00008002;
const int CONSTANT_ALPHA = 0x00008003;
const int ONE_MINUS_CONSTANT_ALPHA = 0x00008004;
const int FUNC_ADD = 0x00008006;
const int FUNC_REVERSE_SUBTRACT = 0x0000800B;
const int FUNC_SUBTRACT = 0x0000800A;
const int MIN = 0x00008007;
const int MAX = 0x00008008;
const int BUFFER_SIZE = 0x00008764;
const int BUFFER_USAGE = 0x00008765;
const int QUERY_COUNTER_BITS = 0x00008864;
const int CURRENT_QUERY = 0x00008865;
const int QUERY_RESULT = 0x00008866;
const int QUERY_RESULT_AVAILABLE = 0x00008867;
const int ARRAY_BUFFER = 0x00008892;
const int ELEMENT_ARRAY_BUFFER = 0x00008893;
const int ARRAY_BUFFER_BINDING = 0x00008894;
const int ELEMENT_ARRAY_BUFFER_BINDING = 0x00008895;
const int VERTEX_ATTRIB_ARRAY_BUFFER_BINDING = 0x0000889F;
const int READ_ONLY = 0x000088B8;
const int WRITE_ONLY = 0x000088B9;
const int READ_WRITE = 0x000088BA;
const int BUFFER_ACCESS = 0x000088BB;
const int BUFFER_MAPPED = 0x000088BC;
const int BUFFER_MAP_POINTER = 0x000088BD;
const int STREAM_DRAW = 0x000088E0;
const int STREAM_READ = 0x000088E1;
const int STREAM_COPY = 0x000088E2;
const int STATIC_DRAW = 0x000088E4;
const int STATIC_READ = 0x000088E5;
const int STATIC_COPY = 0x000088E6;
const int DYNAMIC_DRAW = 0x000088E8;
const int DYNAMIC_READ = 0x000088E9;
const int DYNAMIC_COPY = 0x000088EA;
const int SAMPLES_PASSED = 0x00008914;
const int SRC1_ALPHA = 0x00008589;
const int BLEND_EQUATION_RGB = 0x00008009;
const int VERTEX_ATTRIB_ARRAY_ENABLED = 0x00008622;
const int VERTEX_ATTRIB_ARRAY_SIZE = 0x00008623;
const int VERTEX_ATTRIB_ARRAY_STRIDE = 0x00008624;
const int VERTEX_ATTRIB_ARRAY_TYPE = 0x00008625;
const int CURRENT_VERTEX_ATTRIB = 0x00008626;
const int VERTEX_PROGRAM_POINT_SIZE = 0x00008642;
const int VERTEX_ATTRIB_ARRAY_POINTER = 0x00008645;
const int STENCIL_BACK_FUNC = 0x00008800;
const int STENCIL_BACK_FAIL = 0x00008801;
const int STENCIL_BACK_PASS_DEPTH_FAIL = 0x00008802;
const int STENCIL_BACK_PASS_DEPTH_PASS = 0x00008803;
const int MAX_DRAW_BUFFERS = 0x00008824;
const int DRAW_BUFFER0 = 0x00008825;
const int DRAW_BUFFER1 = 0x00008826;
const int DRAW_BUFFER2 = 0x00008827;
const int DRAW_BUFFER3 = 0x00008828;
const int DRAW_BUFFER4 = 0x00008829;
const int DRAW_BUFFER5 = 0x0000882A;
const int DRAW_BUFFER6 = 0x0000882B;
const int DRAW_BUFFER7 = 0x0000882C;
const int DRAW_BUFFER8 = 0x0000882D;
const int DRAW_BUFFER9 = 0x0000882E;
const int DRAW_BUFFER10 = 0x0000882F;
const int DRAW_BUFFER11 = 0x00008830;
const int DRAW_BUFFER12 = 0x00008831;
const int DRAW_BUFFER13 = 0x00008832;
const int DRAW_BUFFER14 = 0x00008833;
const int DRAW_BUFFER15 = 0x00008834;
const int BLEND_EQUATION_ALPHA = 0x0000883D;
const int MAX_VERTEX_ATTRIBS = 0x00008869;
const int VERTEX_ATTRIB_ARRAY_NORMALIZED = 0x0000886A;
const int MAX_TEXTURE_IMAGE_UNITS = 0x00008872;
const int FRAGMENT_SHADER = 0x00008B30;
const int VERTEX_SHADER = 0x00008B31;
const int MAX_FRAGMENT_UNIFORM_COMPONENTS = 0x00008B49;
const int MAX_VERTEX_UNIFORM_COMPONENTS = 0x00008B4A;
const int MAX_VARYING_singleS = 0x00008B4B;
const int MAX_VERTEX_TEXTURE_IMAGE_UNITS = 0x00008B4C;
const int MAX_COMBINED_TEXTURE_IMAGE_UNITS = 0x00008B4D;
const int SHADER_TYPE = 0x00008B4F;
const int single_VEC2 = 0x00008B50;
const int single_VEC3 = 0x00008B51;
const int single_VEC4 = 0x00008B52;
const int INT_VEC2 = 0x00008B53;
const int INT_VEC3 = 0x00008B54;
const int INT_VEC4 = 0x00008B55;
const int BOOL = 0x00008B56;
const int BOOL_VEC2 = 0x00008B57;
const int BOOL_VEC3 = 0x00008B58;
const int BOOL_VEC4 = 0x00008B59;
const int single_MAT2 = 0x00008B5A;
const int single_MAT3 = 0x00008B5B;
const int single_MAT4 = 0x00008B5C;
const int SAMPLER_1D = 0x00008B5D;
const int SAMPLER_2D = 0x00008B5E;
const int SAMPLER_3D = 0x00008B5F;
const int SAMPLER_CUBE = 0x00008B60;
const int SAMPLER_1D_SHADOW = 0x00008B61;
const int SAMPLER_2D_SHADOW = 0x00008B62;
const int DELETE_STATUS = 0x00008B80;
const int COMPILE_STATUS = 0x00008B81;
const int LINK_STATUS = 0x00008B82;
const int VALIDATE_STATUS = 0x00008B83;
const int INFO_LOG_LENGTH = 0x00008B84;
const int ATTACHED_SHADERS = 0x00008B85;
const int ACTIVE_UNIFORMS = 0x00008B86;
const int ACTIVE_UNIFORM_MAX_LENGTH = 0x00008B87;
const int SHADER_SOURCE_LENGTH = 0x00008B88;
const int ACTIVE_ATTRIBUTES = 0x00008B89;
const int ACTIVE_ATTRIBUTE_MAX_LENGTH = 0x00008B8A;
const int FRAGMENT_SHADER_DERIVATIVE_HINT = 0x00008B8B;
const int SHADING_LANGUAGE_VERSION = 0x00008B8C;
const int CURRENT_PROGRAM = 0x00008B8D;
const int POINT_SPRITE_COORD_ORIGIN = 0x00008CA0;
const int LOWER_LEFT = 0x00008CA1;
const int UPPER_LEFT = 0x00008CA2;
const int STENCIL_BACK_REF = 0x00008CA3;
const int STENCIL_BACK_VALUE_MASK = 0x00008CA4;
const int STENCIL_BACK_WRITEMASK = 0x00008CA5;
const int PIXEL_PACK_BUFFER = 0x000088EB;
const int PIXEL_UNPACK_BUFFER = 0x000088EC;
const int PIXEL_PACK_BUFFER_BINDING = 0x000088ED;
const int PIXEL_UNPACK_BUFFER_BINDING = 0x000088EF;
const int single_MAT2x3 = 0x00008B65;
const int single_MAT2x4 = 0x00008B66;
const int single_MAT3x2 = 0x00008B67;
const int single_MAT3x4 = 0x00008B68;
const int single_MAT4x2 = 0x00008B69;
const int single_MAT4x3 = 0x00008B6A;
const int SRGB = 0x00008C40;
const int SRGB8 = 0x00008C41;
const int SRGB_ALPHA = 0x00008C42;
const int SRGB8_ALPHA8 = 0x00008C43;
const int COMPRESSED_SRGB = 0x00008C48;
const int COMPRESSED_SRGB_ALPHA = 0x00008C49;
const int COMPARE_REF_TO_TEXTURE = 0x0000884E;
const int CLIP_DISTANCE0 = 0x00003000;
const int CLIP_DISTANCE1 = 0x00003001;
const int CLIP_DISTANCE2 = 0x00003002;
const int CLIP_DISTANCE3 = 0x00003003;
const int CLIP_DISTANCE4 = 0x00003004;
const int CLIP_DISTANCE5 = 0x00003005;
const int CLIP_DISTANCE6 = 0x00003006;
const int CLIP_DISTANCE7 = 0x00003007;
const int MAX_CLIP_DISTANCES = 0x00000D32;
const int MAJOR_VERSION = 0x0000821B;
const int MINOR_VERSION = 0x0000821C;
const int NUM_EXTENSIONS = 0x0000821D;
const int CONTEXT_FLAGS = 0x0000821E;
const int COMPRESSED_RED = 0x00008225;
const int COMPRESSED_RG = 0x00008226;
const int CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT = 0x000000000001;
const int RGBA32F = 0x00008814;
const int RGB32F = 0x00008815;
const int RGBA16F = 0x0000881A;
const int RGB16F = 0x0000881B;
const int VERTEX_ATTRIB_ARRAY_int = 0x000088FD;
const int MAX_ARRAY_TEXTURE_LAYERS = 0x000088FF;
const int MIN_PROGRAM_TEXEL_OFFSET = 0x00008904;
const int MAX_PROGRAM_TEXEL_OFFSET = 0x00008905;
const int CLAMP_READ_COLOR = 0x0000891C;
const int FIXED_ONLY = 0x0000891D;
const int MAX_VARYING_COMPONENTS = 0x00008B4B;
const int TEXTURE_1D_ARRAY = 0x00008C18;
const int PROXY_TEXTURE_1D_ARRAY = 0x00008C19;
const int TEXTURE_2D_ARRAY = 0x00008C1A;
const int PROXY_TEXTURE_2D_ARRAY = 0x00008C1B;
const int TEXTURE_BINDING_1D_ARRAY = 0x00008C1C;
const int TEXTURE_BINDING_2D_ARRAY = 0x00008C1D;
const int R11F_G11F_B10F = 0x00008C3A;
const int UNSIGNED_INT_10F_11F_11F_REV = 0x00008C3B;
const int RGB9_E5 = 0x00008C3D;
const int UNSIGNED_INT_5_9_9_9_REV = 0x00008C3E;
const int TEXTURE_SHARED_SIZE = 0x00008C3F;
const int TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH = 0x00008C76;
const int TRANSFORM_FEEDBACK_BUFFER_MODE = 0x00008C7F;
const int MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS = 0x00008C80;
const int TRANSFORM_FEEDBACK_VARYINGS = 0x00008C83;
const int TRANSFORM_FEEDBACK_BUFFER_START = 0x00008C84;
const int TRANSFORM_FEEDBACK_BUFFER_SIZE = 0x00008C85;
const int PRIMITIVES_GENERATED = 0x00008C87;
const int TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN = 0x00008C88;
const int RASTERIZER_DISCARD = 0x00008C89;
const int MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x00008C8A;
const int MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS = 0x00008C8B;
const int INTERLEAVED_ATTRIBS = 0x00008C8C;
const int SEPARATE_ATTRIBS = 0x00008C8D;
const int TRANSFORM_FEEDBACK_BUFFER = 0x00008C8E;
const int TRANSFORM_FEEDBACK_BUFFER_BINDING = 0x00008C8F;
const int RGBA32UI = 0x00008D70;
const int RGB32UI = 0x00008D71;
const int RGBA16UI = 0x00008D76;
const int RGB16UI = 0x00008D77;
const int RGBA8UI = 0x00008D7C;
const int RGB8UI = 0x00008D7D;
const int RGBA32I = 0x00008D82;
const int RGB32I = 0x00008D83;
const int RGBA16I = 0x00008D88;
const int RGB16I = 0x00008D89;
const int RGBA8I = 0x00008D8E;
const int RGB8I = 0x00008D8F;
const int RED_int = 0x00008D94;
const int GREEN_int = 0x00008D95;
const int BLUE_int = 0x00008D96;
const int RGB_int = 0x00008D98;
const int RGBA_int = 0x00008D99;
const int BGR_int = 0x00008D9A;
const int BGRA_int = 0x00008D9B;
const int SAMPLER_1D_ARRAY = 0x00008DC0;
const int SAMPLER_2D_ARRAY = 0x00008DC1;
const int SAMPLER_1D_ARRAY_SHADOW = 0x00008DC3;
const int SAMPLER_2D_ARRAY_SHADOW = 0x00008DC4;
const int SAMPLER_CUBE_SHADOW = 0x00008DC5;
const int UNSIGNED_INT_VEC2 = 0x00008DC6;
const int UNSIGNED_INT_VEC3 = 0x00008DC7;
const int UNSIGNED_INT_VEC4 = 0x00008DC8;
const int INT_SAMPLER_1D = 0x00008DC9;
const int INT_SAMPLER_2D = 0x00008DCA;
const int INT_SAMPLER_3D = 0x00008DCB;
const int INT_SAMPLER_CUBE = 0x00008DCC;
const int INT_SAMPLER_1D_ARRAY = 0x00008DCE;
const int INT_SAMPLER_2D_ARRAY = 0x00008DCF;
const int UNSIGNED_INT_SAMPLER_1D = 0x00008DD1;
const int UNSIGNED_INT_SAMPLER_2D = 0x00008DD2;
const int UNSIGNED_INT_SAMPLER_3D = 0x00008DD3;
const int UNSIGNED_INT_SAMPLER_CUBE = 0x00008DD4;
const int UNSIGNED_INT_SAMPLER_1D_ARRAY = 0x00008DD6;
const int UNSIGNED_INT_SAMPLER_2D_ARRAY = 0x00008DD7;
const int QUERY_WAIT = 0x00008E13;
const int QUERY_NO_WAIT = 0x00008E14;
const int QUERY_BY_REGION_WAIT = 0x00008E15;
const int QUERY_BY_REGION_NO_WAIT = 0x00008E16;
const int BUFFER_ACCESS_FLAGS = 0x0000911F;
const int BUFFER_MAP_LENGTH = 0x00009120;
const int BUFFER_MAP_OFFSET = 0x00009121;
const int DEPTH_COMPONENT32F = 0x00008CAC;
const int DEPTH32F_STENCIL8 = 0x00008CAD;
const int single_32_UNSIGNED_INT_24_8_REV = 0x00008DAD;
const int INVALID_FRAMEBUFFER_OPERATION = 0x00000506;
const int FRAMEBUFFER_ATTACHMENT_COLOR_ENCODING = 0x00008210;
const int FRAMEBUFFER_ATTACHMENT_COMPONENT_TYPE = 0x00008211;
const int FRAMEBUFFER_ATTACHMENT_RED_SIZE = 0x00008212;
const int FRAMEBUFFER_ATTACHMENT_GREEN_SIZE = 0x00008213;
const int FRAMEBUFFER_ATTACHMENT_BLUE_SIZE = 0x00008214;
const int FRAMEBUFFER_ATTACHMENT_ALPHA_SIZE = 0x00008215;
const int FRAMEBUFFER_ATTACHMENT_DEPTH_SIZE = 0x00008216;
const int FRAMEBUFFER_ATTACHMENT_STENCIL_SIZE = 0x00008217;
const int FRAMEBUFFER_DEFAULT = 0x00008218;
const int FRAMEBUFFER_UNDEFINED = 0x00008219;
const int DEPTH_STENCIL_ATTACHMENT = 0x0000821A;
const int MAX_RENDERBUFFER_SIZE = 0x000084E8;
const int DEPTH_STENCIL = 0x000084F9;
const int UNSIGNED_INT_24_8 = 0x000084FA;
const int DEPTH24_STENCIL8 = 0x000088F0;
const int TEXTURE_STENCIL_SIZE = 0x000088F1;
const int TEXTURE_RED_TYPE = 0x00008C10;
const int TEXTURE_GREEN_TYPE = 0x00008C11;
const int TEXTURE_BLUE_TYPE = 0x00008C12;
const int TEXTURE_ALPHA_TYPE = 0x00008C13;
const int TEXTURE_DEPTH_TYPE = 0x00008C16;
const int UNSIGNED_NORMALIZED = 0x00008C17;
const int FRAMEBUFFER_BINDING = 0x00008CA6;
const int DRAW_FRAMEBUFFER_BINDING = 0x00008CA6;
const int RENDERBUFFER_BINDING = 0x00008CA7;
const int READ_FRAMEBUFFER = 0x00008CA8;
const int DRAW_FRAMEBUFFER = 0x00008CA9;
const int READ_FRAMEBUFFER_BINDING = 0x00008CAA;
const int RENDERBUFFER_SAMPLES = 0x00008CAB;
const int FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE = 0x00008CD0;
const int FRAMEBUFFER_ATTACHMENT_OBJECT_NAME = 0x00008CD1;
const int FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL = 0x00008CD2;
const int FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE = 0x00008CD3;
const int FRAMEBUFFER_ATTACHMENT_TEXTURE_LAYER = 0x00008CD4;
const int FRAMEBUFFER_COMPLETE = 0x00008CD5;
const int FRAMEBUFFER_INCOMPLETE_ATTACHMENT = 0x00008CD6;
const int FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT = 0x00008CD7;
const int FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER = 0x00008CDB;
const int FRAMEBUFFER_INCOMPLETE_READ_BUFFER = 0x00008CDC;
const int FRAMEBUFFER_UNSUPPORTED = 0x00008CDD;
const int MAX_COLOR_ATTACHMENTS = 0x00008CDF;
const int COLOR_ATTACHMENT0 = 0x00008CE0;
const int COLOR_ATTACHMENT1 = 0x00008CE1;
const int COLOR_ATTACHMENT2 = 0x00008CE2;
const int COLOR_ATTACHMENT3 = 0x00008CE3;
const int COLOR_ATTACHMENT4 = 0x00008CE4;
const int COLOR_ATTACHMENT5 = 0x00008CE5;
const int COLOR_ATTACHMENT6 = 0x00008CE6;
const int COLOR_ATTACHMENT7 = 0x00008CE7;
const int COLOR_ATTACHMENT8 = 0x00008CE8;
const int COLOR_ATTACHMENT9 = 0x00008CE9;
const int COLOR_ATTACHMENT10 = 0x00008CEA;
const int COLOR_ATTACHMENT11 = 0x00008CEB;
const int COLOR_ATTACHMENT12 = 0x00008CEC;
const int COLOR_ATTACHMENT13 = 0x00008CED;
const int COLOR_ATTACHMENT14 = 0x00008CEE;
const int COLOR_ATTACHMENT15 = 0x00008CEF;
const int COLOR_ATTACHMENT16 = 0x00008CF0;
const int COLOR_ATTACHMENT17 = 0x00008CF1;
const int COLOR_ATTACHMENT18 = 0x00008CF2;
const int COLOR_ATTACHMENT19 = 0x00008CF3;
const int COLOR_ATTACHMENT20 = 0x00008CF4;
const int COLOR_ATTACHMENT21 = 0x00008CF5;
const int COLOR_ATTACHMENT22 = 0x00008CF6;
const int COLOR_ATTACHMENT23 = 0x00008CF7;
const int COLOR_ATTACHMENT24 = 0x00008CF8;
const int COLOR_ATTACHMENT25 = 0x00008CF9;
const int COLOR_ATTACHMENT26 = 0x00008CFA;
const int COLOR_ATTACHMENT27 = 0x00008CFB;
const int COLOR_ATTACHMENT28 = 0x00008CFC;
const int COLOR_ATTACHMENT29 = 0x00008CFD;
const int COLOR_ATTACHMENT30 = 0x00008CFE;
const int COLOR_ATTACHMENT31 = 0x00008CFF;
const int DEPTH_ATTACHMENT = 0x00008D00;
const int STENCIL_ATTACHMENT = 0x00008D20;
const int FRAMEBUFFER = 0x00008D40;
const int RENDERBUFFER = 0x00008D41;
const int RENDERBUFFER_WIDTH = 0x00008D42;
const int RENDERBUFFER_HEIGHT = 0x00008D43;
const int RENDERBUFFER_INTERNAL_FORMAT = 0x00008D44;
const int STENCIL_INDEX1 = 0x00008D46;
const int STENCIL_INDEX4 = 0x00008D47;
const int STENCIL_INDEX8 = 0x00008D48;
const int STENCIL_INDEX16 = 0x00008D49;
const int RENDERBUFFER_RED_SIZE = 0x00008D50;
const int RENDERBUFFER_GREEN_SIZE = 0x00008D51;
const int RENDERBUFFER_BLUE_SIZE = 0x00008D52;
const int RENDERBUFFER_ALPHA_SIZE = 0x00008D53;
const int RENDERBUFFER_DEPTH_SIZE = 0x00008D54;
const int RENDERBUFFER_STENCIL_SIZE = 0x00008D55;
const int FRAMEBUFFER_INCOMPLETE_MULTISAMPLE = 0x00008D56;
const int MAX_SAMPLES = 0x00008D57;
const int FRAMEBUFFER_SRGB = 0x00008DB9;
const int HALF_single = 0x0000140B;
const int MAP_READ_BIT = 0x00000001;
const int MAP_WRITE_BIT = 0x00000002;
const int MAP_INVALIDATE_RANGE_BIT = 0x00000004;
const int MAP_INVALIDATE_BUFFER_BIT = 0x00000008;
const int MAP_FLUSH_EXPLICIT_BIT = 0x00000010;
const int MAP_UNSYNCHRONIZED_BIT = 0x00000020;
const int COMPRESSED_RED_RGTC1 = 0x00008DBB;
const int COMPRESSED_SIGNED_RED_RGTC1 = 0x00008DBC;
const int COMPRESSED_RG_RGTC2 = 0x00008DBD;
const int COMPRESSED_SIGNED_RG_RGTC2 = 0x00008DBE;
const int RG = 0x00008227;
const int RG_int = 0x00008228;
const int R8 = 0x00008229;
const int R16 = 0x0000822A;
const int RG8 = 0x0000822B;
const int RG16 = 0x0000822C;
const int R16F = 0x0000822D;
const int R32F = 0x0000822E;
const int RG16F = 0x0000822F;
const int RG32F = 0x00008230;
const int R8I = 0x00008231;
const int R8UI = 0x00008232;
const int R16I = 0x00008233;
const int R16UI = 0x00008234;
const int R32I = 0x00008235;
const int R32UI = 0x00008236;
const int RG8I = 0x00008237;
const int RG8UI = 0x00008238;
const int RG16I = 0x00008239;
const int RG16UI = 0x0000823A;
const int RG32I = 0x0000823B;
const int RG32UI = 0x0000823C;
const int VERTEX_ARRAY_BINDING = 0x000085B5;
 // Public Const VERSION_3_2 1
 //
 // Public Const CONTEXT_CORE_PROFILE_BIT As int  = 0x000000000001
 // Public Const CONTEXT_COMPATIBILITY_PROFILE_BIT As int  = 0x000000000002
 // Public Const LINES_ADJACENCY As int  = 0x0000000A
 // Public Const LINE_STRIP_ADJACENCY As int  = 0x0000000B
 // Public Const TRIANGLES_ADJACENCY As int  = 0x0000000C
 // Public Const TRIANGLE_STRIP_ADJACENCY As int  = 0x0000000D
 // Public Const PROGRAM_POINT_SIZE As int  = 0x00008642
 // Public Const MAX_GEOMETRY_TEXTURE_IMAGE_UNITS As int  = 0x00008C29
 // Public Const FRAMEBUFFER_ATTACHMENT_LAYERED As int  = 0x00008DA7
 // Public Const FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS As int  = 0x00008DA8
 // Public Const GEOMETRY_SHADER As int  = 0x00008DD9
 // Public Const GEOMETRY_VERTICES_OUT As int  = 0x00008916
 // Public Const GEOMETRY_INPUT_TYPE As int  = 0x00008917
 // Public Const GEOMETRY_OUTPUT_TYPE As int  = 0x00008918
 // Public Const MAX_GEOMETRY_UNIFORM_COMPONENTS As int  = 0x00008DDF
 // Public Const MAX_GEOMETRY_OUTPUT_VERTICES As int  = 0x00008DE0
 // Public Const MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS As int  = 0x00008DE1
 // Public Const MAX_VERTEX_OUTPUT_COMPONENTS As int  = 0x00009122
 // Public Const MAX_GEOMETRY_INPUT_COMPONENTS As int  = 0x00009123
 // Public Const MAX_GEOMETRY_OUTPUT_COMPONENTS As int  = 0x00009124
 // Public Const MAX_FRAGMENT_INPUT_COMPONENTS As int  = 0x00009125
 // Public Const CONTEXT_PROFILE_MASK As int  = 0x00009126
 // Public Const DEPTH_CLAMP As int  = 0x0000864F
 // Public Const QUADS_FOLLOW_PROVOKING_VERTEX_CONVENTION As int  = 0x00008E4C
 // Public Const FIRST_VERTEX_CONVENTION As int  = 0x00008E4D
 // Public Const LAST_VERTEX_CONVENTION As int  = 0x00008E4E
 // Public Const PROVOKING_VERTEX As int  = 0x00008E4F
 // Public Const TEXTURE_CUBE_MAP_SEAMLESS As int  = 0x0000884F
 // Public Const MAX_SERVER_WAIT_TIMEOUT As int  = 0x00009111
 // Public Const OBJECT_TYPE As int  = 0x00009112
 // Public Const SYNC_CONDITION As int  = 0x00009113
 // Public Const SYNC_STATUS As int  = 0x00009114
 // Public Const SYNC_FLAGS As int  = 0x00009115
 // Public Const SYNC_FENCE As int  = 0x00009116
 // Public Const SYNC_GPU_COMMANDS_COMPLETE As int  = 0x00009117
 // Public Const UNSIGNALED As int  = 0x00009118
 // Public Const SIGNALED As int  = 0x00009119
 // Public Const ALREADY_SIGNALED As int  = 0x0000911A
 // Public Const TIMEOUT_EXPIRED As int  = 0x0000911B
 // Public Const CONDITION_SATISFIED As int  = 0x0000911C
 // Public Const WAIT_FAILED As int  = 0x0000911D
 // Public Const TIMEOUT_IGNORED As int = & HFFFFFFFFFFFFFFFFull
 // Public Const SYNC_FLUSH_COMMANDS_BIT As int  = 0x000000000001
 // Public Const SAMPLE_POSITION As int  = 0x00008E50
 // Public Const SAMPLE_MASK As int  = 0x00008E51
 // Public Const SAMPLE_MASK_VALUE As int  = 0x00008E52
 // Public Const MAX_SAMPLE_MASK_WORDS As int  = 0x00008E59
 // Public Const TEXTURE_2D_MULTISAMPLE As int  = 0x00009100
 // Public Const PROXY_TEXTURE_2D_MULTISAMPLE As int  = 0x00009101
 // Public Const TEXTURE_2D_MULTISAMPLE_ARRAY As int  = 0x00009102
 // Public Const PROXY_TEXTURE_2D_MULTISAMPLE_ARRAY As int  = 0x00009103
 // Public Const TEXTURE_BINDING_2D_MULTISAMPLE As int  = 0x00009104
 // Public Const TEXTURE_BINDING_2D_MULTISAMPLE_ARRAY As int  = 0x00009105
 // Public Const TEXTURE_SAMPLES As int  = 0x00009106
 // Public Const TEXTURE_FIXED_SAMPLE_LOCATIONS As int  = 0x00009107
 // Public Const SAMPLER_2D_MULTISAMPLE As int  = 0x00009108
 // Public Const INT_SAMPLER_2D_MULTISAMPLE As int  = 0x00009109
 // Public Const UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE As int  = 0x0000910A
 // Public Const SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910B
 // Public Const INT_SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910C
 // Public Const UNSIGNED_INT_SAMPLER_2D_MULTISAMPLE_ARRAY As int  = 0x0000910D
 // Public Const MAX_COLOR_TEXTURE_SAMPLES As int  = 0x0000910E
 // Public Const MAX_DEPTH_TEXTURE_SAMPLES As int  = 0x0000910F
 // Public Const MAX_int_SAMPLES As int  = 0x00009110
 // Public Const VERSION_3_1 1
 // Public Const SAMPLER_2D_RECT As int  = 0x00008B63
 // Public Const SAMPLER_2D_RECT_SHADOW As int  = 0x00008B64
 // Public Const SAMPLER_BUFFER As int  = 0x00008DC2
 // Public Const INT_SAMPLER_2D_RECT As int  = 0x00008DCD
 // Public Const INT_SAMPLER_BUFFER As int  = 0x00008DD0
 // Public Const UNSIGNED_INT_SAMPLER_2D_RECT As int  = 0x00008DD5
 // Public Const UNSIGNED_INT_SAMPLER_BUFFER As int  = 0x00008DD8
 // Public Const TEXTURE_BUFFER As int  = 0x00008C2A
 // Public Const MAX_TEXTURE_BUFFER_SIZE As int  = 0x00008C2B
 // Public Const TEXTURE_BINDING_BUFFER As int  = 0x00008C2C
 // Public Const TEXTURE_BUFFER_DATA_STORE_BINDING As int  = 0x00008C2D
 // Public Const TEXTURE_RECTANGLE As int  = 0x000084F5
 // Public Const TEXTURE_BINDING_RECTANGLE As int  = 0x000084F6
 // Public Const PROXY_TEXTURE_RECTANGLE As int  = 0x000084F7
 // Public Const MAX_RECTANGLE_TEXTURE_SIZE As int  = 0x000084F8
 // Public Const R8_SNORM As int  = 0x00008F94
 // Public Const RG8_SNORM As int  = 0x00008F95
 // Public Const RGB8_SNORM As int  = 0x00008F96
 // Public Const RGBA8_SNORM As int  = 0x00008F97
 // Public Const R16_SNORM As int  = 0x00008F98
 // Public Const RG16_SNORM As int  = 0x00008F99
 // Public Const RGB16_SNORM As int  = 0x00008F9A
 // Public Const RGBA16_SNORM As int  = 0x00008F9B
 // Public Const SIGNED_NORMALIZED As int  = 0x00008F9C
 // Public Const PRIMITIVE_RESTART As int  = 0x00008F9D
 // Public Const PRIMITIVE_RESTART_INDEX As int  = 0x00008F9E
 // Public Const COPY_READ_BUFFER As int  = 0x00008F36
 // Public Const COPY_WRITE_BUFFER As int  = 0x00008F37
 // Public Const UNIFORM_BUFFER As int  = 0x00008A11
 // Public Const UNIFORM_BUFFER_BINDING As int  = 0x00008A28
 // Public Const UNIFORM_BUFFER_START As int  = 0x00008A29
 // Public Const UNIFORM_BUFFER_SIZE As int  = 0x00008A2A
 // Public Const MAX_VERTEX_UNIFORM_BLOCKS As int  = 0x00008A2B
 // Public Const MAX_GEOMETRY_UNIFORM_BLOCKS As int  = 0x00008A2C
 // Public Const MAX_FRAGMENT_UNIFORM_BLOCKS As int  = 0x00008A2D
 // Public Const MAX_COMBINED_UNIFORM_BLOCKS As int  = 0x00008A2E
 // Public Const MAX_UNIFORM_BUFFER_BINDINGS As int  = 0x00008A2F
 // Public Const MAX_UNIFORM_BLOCK_SIZE As int  = 0x00008A30
 // Public Const MAX_COMBINED_VERTEX_UNIFORM_COMPONENTS As int  = 0x00008A31
 // Public Const MAX_COMBINED_GEOMETRY_UNIFORM_COMPONENTS As int  = 0x00008A32
 // Public Const MAX_COMBINED_FRAGMENT_UNIFORM_COMPONENTS As int  = 0x00008A33
 // Public Const UNIFORM_BUFFER_OFFSET_ALIGNMENT As int  = 0x00008A34
 // Public Const ACTIVE_UNIFORM_BLOCK_MAX_NAME_LENGTH As int  = 0x00008A35
 // Public Const ACTIVE_UNIFORM_BLOCKS As int  = 0x00008A36
 // Public Const UNIFORM_TYPE As int  = 0x00008A37
 // Public Const UNIFORM_SIZE As int  = 0x00008A38
 // Public Const UNIFORM_NAME_LENGTH As int  = 0x00008A39
 // Public Const UNIFORM_BLOCK_INDEX As int  = 0x00008A3A
 // Public Const UNIFORM_OFFSET As int  = 0x00008A3B
 // Public Const UNIFORM_ARRAY_STRIDE As int  = 0x00008A3C
 // Public Const UNIFORM_MATRIX_STRIDE As int  = 0x00008A3D
 // Public Const UNIFORM_IS_ROW_MAJOR As int  = 0x00008A3E
 // Public Const UNIFORM_BLOCK_BINDING As int  = 0x00008A3F
 // Public Const UNIFORM_BLOCK_DATA_SIZE As int  = 0x00008A40
 // Public Const UNIFORM_BLOCK_NAME_LENGTH As int  = 0x00008A41
 // Public Const UNIFORM_BLOCK_ACTIVE_UNIFORMS As int  = 0x00008A42
 // Public Const UNIFORM_BLOCK_ACTIVE_UNIFORM_INDICES As int  = 0x00008A43
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_VERTEX_SHADER As int  = 0x00008A44
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_GEOMETRY_SHADER As int  = 0x00008A45
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_FRAGMENT_SHADER As int  = 0x00008A46
 // Public Const INVALID_INDEX As int = & HFFFFFFFFu
 // Public Const VERSION_4_3 1
 // Public Const NUM_SHADING_LANGUAGE_VERSIONS As int  = 0x000082E9
 // Public Const VERTEX_ATTRIB_ARRAY_LONG As int  = 0x0000874E
 // Public Const COMPRESSED_RGB8_ETC2 As int  = 0x00009274
 // Public Const COMPRESSED_SRGB8_ETC2 As int  = 0x00009275
 // Public Const COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2 As int  = 0x00009276
 // Public Const COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2 As int  = 0x00009277
 // Public Const COMPRESSED_RGBA8_ETC2_EAC As int  = 0x00009278
 // Public Const COMPRESSED_SRGB8_ALPHA8_ETC2_EAC As int  = 0x00009279
 // Public Const COMPRESSED_R11_EAC As int  = 0x00009270
 // Public Const COMPRESSED_SIGNED_R11_EAC As int  = 0x00009271
 // Public Const COMPRESSED_RG11_EAC As int  = 0x00009272
 // Public Const COMPRESSED_SIGNED_RG11_EAC As int  = 0x00009273
 // Public Const PRIMITIVE_RESTART_FIXED_INDEX As int  = 0x00008D69
 // Public Const ANY_SAMPLES_PASSED_CONSERVATIVE As int  = 0x00008D6A
 // Public Const MAX_ELEMENT_INDEX As int  = 0x00008D6B
 // Public Const COMPUTE_SHADER As int  = 0x000091B9
 // Public Const MAX_COMPUTE_UNIFORM_BLOCKS As int  = 0x000091BB
 // Public Const MAX_COMPUTE_TEXTURE_IMAGE_UNITS As int  = 0x000091BC
 // Public Const MAX_COMPUTE_IMAGE_UNIFORMS As int  = 0x000091BD
 // Public Const MAX_COMPUTE_SHARED_MEMORY_SIZE As int  = 0x00008262
 // Public Const MAX_COMPUTE_UNIFORM_COMPONENTS As int  = 0x00008263
 // Public Const MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS As int  = 0x00008264
 // Public Const MAX_COMPUTE_ATOMIC_COUNTERS As int  = 0x00008265
 // Public Const MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS As int  = 0x00008266
 // Public Const MAX_COMPUTE_WORK_GROUP_INVOCATIONS As int  = 0x000090EB
 // Public Const MAX_COMPUTE_WORK_GROUP_COUNT As int  = 0x000091BE
 // Public Const MAX_COMPUTE_WORK_GROUP_SIZE As int  = 0x000091BF
 // Public Const COMPUTE_WORK_GROUP_SIZE As int  = 0x00008267
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER As int  = 0x000090EC
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER As int  = 0x000090ED
 // Public Const DISPATCH_INDIRECT_BUFFER As int  = 0x000090EE
 // Public Const DISPATCH_INDIRECT_BUFFER_BINDING As int  = 0x000090EF
 // Public Const COMPUTE_SHADER_BIT As int  = 0x000000000020
 // Public Const Console.WriteLine(_OUTPUT_SYNCHRONOUS As int  = 0x00008242
 // Public Const Console.WriteLine(_NEXT_LOGGED_MESSAGE_LENGTH As int  = 0x00008243
 // Public Const Console.WriteLine(_CALLBACK_FUNCTION As int  = 0x00008244
 // Public Const Console.WriteLine(_CALLBACK_USER_PARAM As int  = 0x00008245
 // Public Const Console.WriteLine(_SOURCE_API As int  = 0x00008246
 // Public Const Console.WriteLine(_SOURCE_WINDOW_SYSTEM As int  = 0x00008247
 // Public Const Console.WriteLine(_SOURCE_SHADER_COMPILER As int  = 0x00008248
 // Public Const Console.WriteLine(_SOURCE_THIRD_PARTY As int  = 0x00008249
 // Public Const Console.WriteLine(_SOURCE_APPLICATION As int  = 0x0000824A
 // Public Const Console.WriteLine(_SOURCE_OTHER As int  = 0x0000824B
 // Public Const Console.WriteLine(_TYPE_ERROR As int  = 0x0000824C
 // Public Const Console.WriteLine(_TYPE_DEPRECATED_BEHAVIOR As int  = 0x0000824D
 // Public Const Console.WriteLine(_TYPE_UNDEFINED_BEHAVIOR As int  = 0x0000824E
 // Public Const Console.WriteLine(_TYPE_PORTABILITY As int  = 0x0000824F
 // Public Const Console.WriteLine(_TYPE_PERFORMANCE As int  = 0x00008250
 // Public Const Console.WriteLine(_TYPE_OTHER As int  = 0x00008251
 // Public Const MAX_Console.WriteLine(_MESSAGE_LENGTH As int  = 0x00009143
 // Public Const MAX_Console.WriteLine(_LOGGED_MESSAGES As int  = 0x00009144
 // Public Const Console.WriteLine(_LOGGED_MESSAGES As int  = 0x00009145
 // Public Const Console.WriteLine(_SEVERITY_HIGH As int  = 0x00009146
 // Public Const Console.WriteLine(_SEVERITY_MEDIUM As int  = 0x00009147
 // Public Const Console.WriteLine(_SEVERITY_LOW As int  = 0x00009148
 // Public Const Console.WriteLine(_TYPE_MARKER As int  = 0x00008268
 // Public Const Console.WriteLine(_TYPE_PUSH_GROUP As int  = 0x00008269
 // Public Const Console.WriteLine(_TYPE_POP_GROUP As int  = 0x0000826A
 // Public Const Console.WriteLine(_SEVERITY_NOTIFICATION As int  = 0x0000826B
 // Public Const MAX_Console.WriteLine(_GROUP_STACK_DEPTH As int  = 0x0000826C
 // Public Const Console.WriteLine(_GROUP_STACK_DEPTH As int  = 0x0000826D
 // Public Const BUFFER As int  = 0x000082E0
 // Public Const SHADER As int  = 0x000082E1
 // Public Const PROGRAM As int  = 0x000082E2
 // Public Const QUERY As int  = 0x000082E3
 // Public Const PROGRAM_PIPELINE As int  = 0x000082E4
 // Public Const SAMPLER As int  = 0x000082E6
 // Public Const MAX_LABEL_LENGTH As int  = 0x000082E8
 // Public Const Console.WriteLine(_OUTPUT As int  = 0x000092E0
 // Public Const CONTEXT_FLAG_Console.WriteLine(_BIT As int  = 0x000000000002
 // Public Const MAX_UNIFORM_LOCATIONS As int  = 0x0000826E
 // Public Const FRAMEBUFFER_DEFAULT_WIDTH As int  = 0x00009310
 // Public Const FRAMEBUFFER_DEFAULT_HEIGHT As int  = 0x00009311
 // Public Const FRAMEBUFFER_DEFAULT_LAYERS As int  = 0x00009312
 // Public Const FRAMEBUFFER_DEFAULT_SAMPLES As int  = 0x00009313
 // Public Const FRAMEBUFFER_DEFAULT_FIXED_SAMPLE_LOCATIONS As int  = 0x00009314
 // Public Const MAX_FRAMEBUFFER_WIDTH As int  = 0x00009315
 // Public Const MAX_FRAMEBUFFER_HEIGHT As int  = 0x00009316
 // Public Const MAX_FRAMEBUFFER_LAYERS As int  = 0x00009317
 // Public Const MAX_FRAMEBUFFER_SAMPLES As int  = 0x00009318
 // Public Const INTERNALFORMAT_SUPPORTED As int  = 0x0000826F
 // Public Const INTERNALFORMAT_PREFERRED As int  = 0x00008270
 // Public Const INTERNALFORMAT_RED_SIZE As int  = 0x00008271
 // Public Const INTERNALFORMAT_GREEN_SIZE As int  = 0x00008272
 // Public Const INTERNALFORMAT_BLUE_SIZE As int  = 0x00008273
 // Public Const INTERNALFORMAT_ALPHA_SIZE As int  = 0x00008274
 // Public Const INTERNALFORMAT_DEPTH_SIZE As int  = 0x00008275
 // Public Const INTERNALFORMAT_STENCIL_SIZE As int  = 0x00008276
 // Public Const INTERNALFORMAT_SHARED_SIZE As int  = 0x00008277
 // Public Const INTERNALFORMAT_RED_TYPE As int  = 0x00008278
 // Public Const INTERNALFORMAT_GREEN_TYPE As int  = 0x00008279
 // Public Const INTERNALFORMAT_BLUE_TYPE As int  = 0x0000827A
 // Public Const INTERNALFORMAT_ALPHA_TYPE As int  = 0x0000827B
 // Public Const INTERNALFORMAT_DEPTH_TYPE As int  = 0x0000827C
 // Public Const INTERNALFORMAT_STENCIL_TYPE As int  = 0x0000827D
 // Public Const MAX_WIDTH As int  = 0x0000827E
 // Public Const MAX_HEIGHT As int  = 0x0000827F
 // Public Const MAX_DEPTH As int  = 0x00008280
 // Public Const MAX_LAYERS As int  = 0x00008281
 // Public Const MAX_COMBINED_DIMENSIONS As int  = 0x00008282
 // Public Const COLOR_COMPONENTS As int  = 0x00008283
 // Public Const DEPTH_COMPONENTS As int  = 0x00008284
 // Public Const STENCIL_COMPONENTS As int  = 0x00008285
 // Public Const COLOR_RENDERABLE As int  = 0x00008286
 // Public Const DEPTH_RENDERABLE As int  = 0x00008287
 // Public Const STENCIL_RENDERABLE As int  = 0x00008288
 // Public Const FRAMEBUFFER_RENDERABLE As int  = 0x00008289
 // Public Const FRAMEBUFFER_RENDERABLE_LAYERED As int  = 0x0000828A
 // Public Const FRAMEBUFFER_BLEND As int  = 0x0000828B
 // Public Const READ_PIXELS As int  = 0x0000828C
 // Public Const READ_PIXELS_FORMAT As int  = 0x0000828D
 // Public Const READ_PIXELS_TYPE As int  = 0x0000828E
 // Public Const TEXTURE_IMAGE_FORMAT As int  = 0x0000828F
 // Public Const TEXTURE_IMAGE_TYPE As int  = 0x00008290
 // Public Const GET_TEXTURE_IMAGE_FORMAT As int  = 0x00008291
 // Public Const GET_TEXTURE_IMAGE_TYPE As int  = 0x00008292
 // Public Const MIPMAP As int  = 0x00008293
 // Public Const MANUAL_GENERATE_MIPMAP As int  = 0x00008294
 // Public Const AUTO_GENERATE_MIPMAP As int  = 0x00008295
 // Public Const COLOR_ENCODING As int  = 0x00008296
 // Public Const SRGB_READ As int  = 0x00008297
 // Public Const SRGB_WRITE As int  = 0x00008298
 // Public Const FILTER As int  = 0x0000829A
 // Public Const VERTEX_TEXTURE As int  = 0x0000829B
 // Public Const TESS_CONTROL_TEXTURE As int  = 0x0000829C
 // Public Const TESS_EVALUATION_TEXTURE As int  = 0x0000829D
 // Public Const GEOMETRY_TEXTURE As int  = 0x0000829E
 // Public Const FRAGMENT_TEXTURE As int  = 0x0000829F
 // Public Const COMPUTE_TEXTURE As int  = 0x000082A0
 // Public Const TEXTURE_SHADOW As int  = 0x000082A1
 // Public Const TEXTURE_GATHER As int  = 0x000082A2
 // Public Const TEXTURE_GATHER_SHADOW As int  = 0x000082A3
 // Public Const SHADER_IMAGE_LOAD As int  = 0x000082A4
 // Public Const SHADER_IMAGE_STORE As int  = 0x000082A5
 // Public Const SHADER_IMAGE_ATOMIC As int  = 0x000082A6
 // Public Const IMAGE_TEXEL_SIZE As int  = 0x000082A7
 // Public Const IMAGE_COMPATIBILITY_CLASS As int  = 0x000082A8
 // Public Const IMAGE_PIXEL_FORMAT As int  = 0x000082A9
 // Public Const IMAGE_PIXEL_TYPE As int  = 0x000082AA
 // Public Const SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST As int  = 0x000082AC
 // Public Const SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST As int  = 0x000082AD
 // Public Const SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE As int  = 0x000082AE
 // Public Const SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE As int  = 0x000082AF
 // Public Const TEXTURE_COMPRESSED_BLOCK_WIDTH As int  = 0x000082B1
 // Public Const TEXTURE_COMPRESSED_BLOCK_HEIGHT As int  = 0x000082B2
 // Public Const TEXTURE_COMPRESSED_BLOCK_SIZE As int  = 0x000082B3
 // Public Const CLEAR_BUFFER As int  = 0x000082B4
 // Public Const TEXTURE_VIEW As int  = 0x000082B5
 // Public Const VIEW_COMPATIBILITY_CLASS As int  = 0x000082B6
 // Public Const FULL_SUPPORT As int  = 0x000082B7
 // Public Const CAVEAT_SUPPORT As int  = 0x000082B8
 // Public Const IMAGE_CLASS_4_X_32 As int  = 0x000082B9
 // Public Const IMAGE_CLASS_2_X_32 As int  = 0x000082BA
 // Public Const IMAGE_CLASS_1_X_32 As int  = 0x000082BB
 // Public Const IMAGE_CLASS_4_X_16 As int  = 0x000082BC
 // Public Const IMAGE_CLASS_2_X_16 As int  = 0x000082BD
 // Public Const IMAGE_CLASS_1_X_16 As int  = 0x000082BE
 // Public Const IMAGE_CLASS_4_X_8 As int  = 0x000082BF
 // Public Const IMAGE_CLASS_2_X_8 As int  = 0x000082C0
 // Public Const IMAGE_CLASS_1_X_8 As int  = 0x000082C1
 // Public Const IMAGE_CLASS_11_11_10 As int  = 0x000082C2
 // Public Const IMAGE_CLASS_10_10_10_2 As int  = 0x000082C3
 // Public Const VIEW_CLASS_128_BITS As int  = 0x000082C4
 // Public Const VIEW_CLASS_96_BITS As int  = 0x000082C5
 // Public Const VIEW_CLASS_64_BITS As int  = 0x000082C6
 // Public Const VIEW_CLASS_48_BITS As int  = 0x000082C7
 // Public Const VIEW_CLASS_32_BITS As int  = 0x000082C8
 // Public Const VIEW_CLASS_24_BITS As int  = 0x000082C9
 // Public Const VIEW_CLASS_16_BITS As int  = 0x000082CA
 // Public Const VIEW_CLASS_8_BITS As int  = 0x000082CB
 // Public Const VIEW_CLASS_S3TC_DXT1_RGB As int  = 0x000082CC
 // Public Const VIEW_CLASS_S3TC_DXT1_RGBA As int  = 0x000082CD
 // Public Const VIEW_CLASS_S3TC_DXT3_RGBA As int  = 0x000082CE
 // Public Const VIEW_CLASS_S3TC_DXT5_RGBA As int  = 0x000082CF
 // Public Const VIEW_CLASS_RGTC1_RED As int  = 0x000082D0
 // Public Const VIEW_CLASS_RGTC2_RG As int  = 0x000082D1
 // Public Const VIEW_CLASS_BPTC_UNORM As int  = 0x000082D2
 // Public Const VIEW_CLASS_BPTC_single As int  = 0x000082D3
 // Public Const UNIFORM As int  = 0x000092E1
 // Public Const UNIFORM_BLOCK As int  = 0x000092E2
 // Public Const PROGRAM_INPUT As int  = 0x000092E3
 // Public Const PROGRAM_OUTPUT As int  = 0x000092E4
 // Public Const BUFFER_VARIABLE As int  = 0x000092E5
 // Public Const SHADER_STORAGE_BLOCK As int  = 0x000092E6
 // Public Const VERTEX_SUBROUTINE As int  = 0x000092E8
 // Public Const TESS_CONTROL_SUBROUTINE As int  = 0x000092E9
 // Public Const TESS_EVALUATION_SUBROUTINE As int  = 0x000092EA
 // Public Const GEOMETRY_SUBROUTINE As int  = 0x000092EB
 // Public Const FRAGMENT_SUBROUTINE As int  = 0x000092EC
 // Public Const COMPUTE_SUBROUTINE As int  = 0x000092ED
 // Public Const VERTEX_SUBROUTINE_UNIFORM As int  = 0x000092EE
 // Public Const TESS_CONTROL_SUBROUTINE_UNIFORM As int  = 0x000092EF
 // Public Const TESS_EVALUATION_SUBROUTINE_UNIFORM As int  = 0x000092F0
 // Public Const GEOMETRY_SUBROUTINE_UNIFORM As int  = 0x000092F1
 // Public Const FRAGMENT_SUBROUTINE_UNIFORM As int  = 0x000092F2
 // Public Const COMPUTE_SUBROUTINE_UNIFORM As int  = 0x000092F3
 // Public Const TRANSFORM_FEEDBACK_VARYING As int  = 0x000092F4
 // Public Const ACTIVE_RESOURCES As int  = 0x000092F5
 // Public Const MAX_NAME_LENGTH As int  = 0x000092F6
 // Public Const MAX_NUM_ACTIVE_VARIABLES As int  = 0x000092F7
 // Public Const MAX_NUM_COMPATIBLE_SUBROUTINES As int  = 0x000092F8
 // Public Const NAME_LENGTH As int  = 0x000092F9
 // Public Const TYPE As int  = 0x000092FA
 // Public Const ARRAY_SIZE As int  = 0x000092FB
 // Public Const OFFSET As int  = 0x000092FC
 // Public Const BLOCK_INDEX As int  = 0x000092FD
 // Public Const ARRAY_STRIDE As int  = 0x000092FE
 // Public Const MATRIX_STRIDE As int  = 0x000092FF
 // Public Const IS_ROW_MAJOR As int  = 0x00009300
 // Public Const ATOMIC_COUNTER_BUFFER_INDEX As int  = 0x00009301
 // Public Const BUFFER_BINDING As int  = 0x00009302
 // Public Const BUFFER_DATA_SIZE As int  = 0x00009303
 // Public Const NUM_ACTIVE_VARIABLES As int  = 0x00009304
 // Public Const ACTIVE_VARIABLES As int  = 0x00009305
 // Public Const REFERENCED_BY_VERTEX_SHADER As int  = 0x00009306
 // Public Const REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x00009307
 // Public Const REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x00009308
 // Public Const REFERENCED_BY_GEOMETRY_SHADER As int  = 0x00009309
 // Public Const REFERENCED_BY_FRAGMENT_SHADER As int  = 0x0000930A
 // Public Const REFERENCED_BY_COMPUTE_SHADER As int  = 0x0000930B
 // Public Const TOP_LEVEL_ARRAY_SIZE As int  = 0x0000930C
 // Public Const TOP_LEVEL_ARRAY_STRIDE As int  = 0x0000930D
 // Public Const LOCATION As int  = 0x0000930E
 // Public Const LOCATION_INDEX As int  = 0x0000930F
 // Public Const IS_PER_PATCH As int  = 0x000092E7
 // Public Const SHADER_STORAGE_BUFFER As int  = 0x000090D2
 // Public Const SHADER_STORAGE_BUFFER_BINDING As int  = 0x000090D3
 // Public Const SHADER_STORAGE_BUFFER_START As int  = 0x000090D4
 // Public Const SHADER_STORAGE_BUFFER_SIZE As int  = 0x000090D5
 // Public Const MAX_VERTEX_SHADER_STORAGE_BLOCKS As int  = 0x000090D6
 // Public Const MAX_GEOMETRY_SHADER_STORAGE_BLOCKS As int  = 0x000090D7
 // Public Const MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS As int  = 0x000090D8
 // Public Const MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS As int  = 0x000090D9
 // Public Const MAX_FRAGMENT_SHADER_STORAGE_BLOCKS As int  = 0x000090DA
 // Public Const MAX_COMPUTE_SHADER_STORAGE_BLOCKS As int  = 0x000090DB
 // Public Const MAX_COMBINED_SHADER_STORAGE_BLOCKS As int  = 0x000090DC
 // Public Const MAX_SHADER_STORAGE_BUFFER_BINDINGS As int  = 0x000090DD
 // Public Const MAX_SHADER_STORAGE_BLOCK_SIZE As int  = 0x000090DE
 // Public Const SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT As int  = 0x000090DF
 // Public Const SHADER_STORAGE_BARRIER_BIT As int  = 0x000000002000
 // Public Const MAX_COMBINED_SHADER_OUTPUT_RESOURCES As int  = 0x00008F39
 // Public Const DEPTH_STENCIL_TEXTURE_MODE As int  = 0x000090EA
 // Public Const TEXTURE_BUFFER_OFFSET As int  = 0x0000919D
 // Public Const TEXTURE_BUFFER_SIZE As int  = 0x0000919E
 // Public Const TEXTURE_BUFFER_OFFSET_ALIGNMENT As int  = 0x0000919F
 // Public Const TEXTURE_VIEW_MIN_LEVEL As int  = 0x000082DB
 // Public Const TEXTURE_VIEW_NUM_LEVELS As int  = 0x000082DC
 // Public Const TEXTURE_VIEW_MIN_LAYER As int  = 0x000082DD
 // Public Const TEXTURE_VIEW_NUM_LAYERS As int  = 0x000082DE
 // Public Const TEXTURE_IMMUTABLE_LEVELS As int  = 0x000082DF
 // Public Const VERTEX_ATTRIB_BINDING As int  = 0x000082D4
 // Public Const VERTEX_ATTRIB_RELATIVE_OFFSET As int  = 0x000082D5
 // Public Const VERTEX_BINDING_DIVISOR As int  = 0x000082D6
 // Public Const VERTEX_BINDING_OFFSET As int  = 0x000082D7
 // Public Const VERTEX_BINDING_STRIDE As int  = 0x000082D8
 // Public Const MAX_VERTEX_ATTRIB_RELATIVE_OFFSET As int  = 0x000082D9
 // Public Const MAX_VERTEX_ATTRIB_BINDINGS As int  = 0x000082DA
 // Public Const VERTEX_BINDING_BUFFER As int  = 0x00008F4F
 // Public Const VERSION_4_2 1
 // Public Const COPY_READ_BUFFER_BINDING As int  = 0x00008F36
 // Public Const COPY_WRITE_BUFFER_BINDING As int  = 0x00008F37
 // Public Const TRANSFORM_FEEDBACK_ACTIVE As int  = 0x00008E24
 // Public Const TRANSFORM_FEEDBACK_PAUSED As int  = 0x00008E23
 // Public Const UNPACK_COMPRESSED_BLOCK_WIDTH As int  = 0x00009127
 // Public Const UNPACK_COMPRESSED_BLOCK_HEIGHT As int  = 0x00009128
 // Public Const UNPACK_COMPRESSED_BLOCK_DEPTH As int  = 0x00009129
 // Public Const UNPACK_COMPRESSED_BLOCK_SIZE As int  = 0x0000912A
 // Public Const PACK_COMPRESSED_BLOCK_WIDTH As int  = 0x0000912B
 // Public Const PACK_COMPRESSED_BLOCK_HEIGHT As int  = 0x0000912C
 // Public Const PACK_COMPRESSED_BLOCK_DEPTH As int  = 0x0000912D
 // Public Const PACK_COMPRESSED_BLOCK_SIZE As int  = 0x0000912E
 // Public Const NUM_SAMPLE_COUNTS As int  = 0x00009380
 // Public Const MIN_MAP_BUFFER_ALIGNMENT As int  = 0x000090BC
 // Public Const ATOMIC_COUNTER_BUFFER As int  = 0x000092C0
 // Public Const ATOMIC_COUNTER_BUFFER_BINDING As int  = 0x000092C1
 // Public Const ATOMIC_COUNTER_BUFFER_START As int  = 0x000092C2
 // Public Const ATOMIC_COUNTER_BUFFER_SIZE As int  = 0x000092C3
 // Public Const ATOMIC_COUNTER_BUFFER_DATA_SIZE As int  = 0x000092C4
 // Public Const ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTERS As int  = 0x000092C5
 // Public Const ATOMIC_COUNTER_BUFFER_ACTIVE_ATOMIC_COUNTER_INDICES As int  = 0x000092C6
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_VERTEX_SHADER As int  = 0x000092C7
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x000092C8
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x000092C9
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_GEOMETRY_SHADER As int  = 0x000092CA
 // Public Const ATOMIC_COUNTER_BUFFER_REFERENCED_BY_FRAGMENT_SHADER As int  = 0x000092CB
 // Public Const MAX_VERTEX_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CC
 // Public Const MAX_TESS_CONTROL_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CD
 // Public Const MAX_TESS_EVALUATION_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CE
 // Public Const MAX_GEOMETRY_ATOMIC_COUNTER_BUFFERS As int  = 0x000092CF
 // Public Const MAX_FRAGMENT_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D0
 // Public Const MAX_COMBINED_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D1
 // Public Const MAX_VERTEX_ATOMIC_COUNTERS As int  = 0x000092D2
 // Public Const MAX_TESS_CONTROL_ATOMIC_COUNTERS As int  = 0x000092D3
 // Public Const MAX_TESS_EVALUATION_ATOMIC_COUNTERS As int  = 0x000092D4
 // Public Const MAX_GEOMETRY_ATOMIC_COUNTERS As int  = 0x000092D5
 // Public Const MAX_FRAGMENT_ATOMIC_COUNTERS As int  = 0x000092D6
 // Public Const MAX_COMBINED_ATOMIC_COUNTERS As int  = 0x000092D7
 // Public Const MAX_ATOMIC_COUNTER_BUFFER_SIZE As int  = 0x000092D8
 // Public Const MAX_ATOMIC_COUNTER_BUFFER_BINDINGS As int  = 0x000092DC
 // Public Const ACTIVE_ATOMIC_COUNTER_BUFFERS As int  = 0x000092D9
 // Public Const UNIFORM_ATOMIC_COUNTER_BUFFER_INDEX As int  = 0x000092DA
 // Public Const UNSIGNED_INT_ATOMIC_COUNTER As int  = 0x000092DB
 // Public Const VERTEX_ATTRIB_ARRAY_BARRIER_BIT As int  = 0x000000000001
 // Public Const ELEMENT_ARRAY_BARRIER_BIT As int  = 0x000000000002
 // Public Const UNIFORM_BARRIER_BIT As int  = 0x000000000004
 // Public Const TEXTURE_FETCH_BARRIER_BIT As int  = 0x000000000008
 // Public Const SHADER_IMAGE_ACCESS_BARRIER_BIT As int  = 0x000000000020
 // Public Const COMMAND_BARRIER_BIT As int  = 0x000000000040
 // Public Const PIXEL_BUFFER_BARRIER_BIT As int  = 0x000000000080
 // Public Const TEXTURE_UPDATE_BARRIER_BIT As int  = 0x000000000100
 // Public Const BUFFER_UPDATE_BARRIER_BIT As int  = 0x000000000200
 // Public Const FRAMEBUFFER_BARRIER_BIT As int  = 0x000000000400
 // Public Const TRANSFORM_FEEDBACK_BARRIER_BIT As int  = 0x000000000800
 // Public Const ATOMIC_COUNTER_BARRIER_BIT As int  = 0x000000001000
 // Public Const ALL_BARRIER_BITS As int  = 0x0000FFFFFFFF
 // Public Const MAX_IMAGE_UNITS As int  = 0x00008F38
 // Public Const MAX_COMBINED_IMAGE_UNITS_AND_FRAGMENT_OUTPUTS As int  = 0x00008F39
 // Public Const IMAGE_BINDING_NAME As int  = 0x00008F3A
 // Public Const IMAGE_BINDING_LEVEL As int  = 0x00008F3B
 // Public Const IMAGE_BINDING_LAYERED As int  = 0x00008F3C
 // Public Const IMAGE_BINDING_LAYER As int  = 0x00008F3D
 // Public Const IMAGE_BINDING_ACCESS As int  = 0x00008F3E
 // Public Const IMAGE_1D As int  = 0x0000904C
 // Public Const IMAGE_2D As int  = 0x0000904D
 // Public Const IMAGE_3D As int  = 0x0000904E
 // Public Const IMAGE_2D_RECT As int  = 0x0000904F
 // Public Const IMAGE_CUBE As int  = 0x00009050
 // Public Const IMAGE_BUFFER As int  = 0x00009051
 // Public Const IMAGE_1D_ARRAY As int  = 0x00009052
 // Public Const IMAGE_2D_ARRAY As int  = 0x00009053
 // Public Const IMAGE_CUBE_MAP_ARRAY As int  = 0x00009054
 // Public Const IMAGE_2D_MULTISAMPLE As int  = 0x00009055
 // Public Const IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x00009056
 // Public Const INT_IMAGE_1D As int  = 0x00009057
 // Public Const INT_IMAGE_2D As int  = 0x00009058
 // Public Const INT_IMAGE_3D As int  = 0x00009059
 // Public Const INT_IMAGE_2D_RECT As int  = 0x0000905A
 // Public Const INT_IMAGE_CUBE As int  = 0x0000905B
 // Public Const INT_IMAGE_BUFFER As int  = 0x0000905C
 // Public Const INT_IMAGE_1D_ARRAY As int  = 0x0000905D
 // Public Const INT_IMAGE_2D_ARRAY As int  = 0x0000905E
 // Public Const INT_IMAGE_CUBE_MAP_ARRAY As int  = 0x0000905F
 // Public Const INT_IMAGE_2D_MULTISAMPLE As int  = 0x00009060
 // Public Const INT_IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x00009061
 // Public Const UNSIGNED_INT_IMAGE_1D As int  = 0x00009062
 // Public Const UNSIGNED_INT_IMAGE_2D As int  = 0x00009063
 // Public Const UNSIGNED_INT_IMAGE_3D As int  = 0x00009064
 // Public Const UNSIGNED_INT_IMAGE_2D_RECT As int  = 0x00009065
 // Public Const UNSIGNED_INT_IMAGE_CUBE As int  = 0x00009066
 // Public Const UNSIGNED_INT_IMAGE_BUFFER As int  = 0x00009067
 // Public Const UNSIGNED_INT_IMAGE_1D_ARRAY As int  = 0x00009068
 // Public Const UNSIGNED_INT_IMAGE_2D_ARRAY As int  = 0x00009069
 // Public Const UNSIGNED_INT_IMAGE_CUBE_MAP_ARRAY As int  = 0x0000906A
 // Public Const UNSIGNED_INT_IMAGE_2D_MULTISAMPLE As int  = 0x0000906B
 // Public Const UNSIGNED_INT_IMAGE_2D_MULTISAMPLE_ARRAY As int  = 0x0000906C
 // Public Const MAX_IMAGE_SAMPLES As int  = 0x0000906D
 // Public Const IMAGE_BINDING_FORMAT As int  = 0x0000906E
 // Public Const IMAGE_FORMAT_COMPATIBILITY_TYPE As int  = 0x000090C7
 // Public Const IMAGE_FORMAT_COMPATIBILITY_BY_SIZE As int  = 0x000090C8
 // Public Const IMAGE_FORMAT_COMPATIBILITY_BY_CLASS As int  = 0x000090C9
 // Public Const MAX_VERTEX_IMAGE_UNIFORMS As int  = 0x000090CA
 // Public Const MAX_TESS_CONTROL_IMAGE_UNIFORMS As int  = 0x000090CB
 // Public Const MAX_TESS_EVALUATION_IMAGE_UNIFORMS As int  = 0x000090CC
 // Public Const MAX_GEOMETRY_IMAGE_UNIFORMS As int  = 0x000090CD
 // Public Const MAX_FRAGMENT_IMAGE_UNIFORMS As int  = 0x000090CE
 // Public Const MAX_COMBINED_IMAGE_UNIFORMS As int  = 0x000090CF
 // Public Const COMPRESSED_RGBA_BPTC_UNORM As int  = 0x00008E8C
 // Public Const COMPRESSED_SRGB_ALPHA_BPTC_UNORM As int  = 0x00008E8D
 // Public Const COMPRESSED_RGB_BPTC_SIGNED_single As int  = 0x00008E8E
 // Public Const COMPRESSED_RGB_BPTC_UNSIGNED_single As int  = 0x00008E8F
 // Public Const TEXTURE_IMMUTABLE_FORMAT As int  = 0x0000912F
 // Public Const VERSION_4_0 1
 // Public Const SAMPLE_SHADING As int  = 0x00008C36
 // Public Const MIN_SAMPLE_SHADING_VALUE As int  = 0x00008C37
 // Public Const MIN_PROGRAM_TEXTURE_GATHER_OFFSET As int  = 0x00008E5E
 // Public Const MAX_PROGRAM_TEXTURE_GATHER_OFFSET As int  = 0x00008E5F
 // Public Const TEXTURE_CUBE_MAP_ARRAY As int  = 0x00009009
 // Public Const TEXTURE_BINDING_CUBE_MAP_ARRAY As int  = 0x0000900A
 // Public Const PROXY_TEXTURE_CUBE_MAP_ARRAY As int  = 0x0000900B
 // Public Const SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900C
 // Public Const SAMPLER_CUBE_MAP_ARRAY_SHADOW As int  = 0x0000900D
 // Public Const INT_SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900E
 // Public Const UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY As int  = 0x0000900F
 // Public Const DRAW_INDIRECT_BUFFER As int  = 0x00008F3F
 // Public Const DRAW_INDIRECT_BUFFER_BINDING As int  = 0x00008F43
 // Public Const GEOMETRY_SHADER_INVOCATIONS As int  = 0x0000887F
 // Public Const MAX_GEOMETRY_SHADER_INVOCATIONS As int  = 0x00008E5A
 // Public Const MIN_FRAGMENT_INTERPOLATION_OFFSET As int  = 0x00008E5B
 // Public Const MAX_FRAGMENT_INTERPOLATION_OFFSET As int  = 0x00008E5C
 // Public Const FRAGMENT_INTERPOLATION_OFFSET_BITS As int  = 0x00008E5D
 // Public Const MAX_VERTEX_STREAMS As int  = 0x00008E71
 // Public Const DOUBLE_VEC2 As int  = 0x00008FFC
 // Public Const DOUBLE_VEC3 As int  = 0x00008FFD
 // Public Const DOUBLE_VEC4 As int  = 0x00008FFE
 // Public Const DOUBLE_MAT2 As int  = 0x00008F46
 // Public Const DOUBLE_MAT3 As int  = 0x00008F47
 // Public Const DOUBLE_MAT4 As int  = 0x00008F48
 // Public Const DOUBLE_MAT2x3 As int  = 0x00008F49
 // Public Const DOUBLE_MAT2x4 As int  = 0x00008F4A
 // Public Const DOUBLE_MAT3x2 As int  = 0x00008F4B
 // Public Const DOUBLE_MAT3x4 As int  = 0x00008F4C
 // Public Const DOUBLE_MAT4x2 As int  = 0x00008F4D
 // Public Const DOUBLE_MAT4x3 As int  = 0x00008F4E
 // Public Const ACTIVE_SUBROUTINES As int  = 0x00008DE5
 // Public Const ACTIVE_SUBROUTINE_UNIFORMS As int  = 0x00008DE6
 // Public Const ACTIVE_SUBROUTINE_UNIFORM_LOCATIONS As int  = 0x00008E47
 // Public Const ACTIVE_SUBROUTINE_MAX_LENGTH As int  = 0x00008E48
 // Public Const ACTIVE_SUBROUTINE_UNIFORM_MAX_LENGTH As int  = 0x00008E49
 // Public Const MAX_SUBROUTINES As int  = 0x00008DE7
 // Public Const MAX_SUBROUTINE_UNIFORM_LOCATIONS As int  = 0x00008DE8
 // Public Const NUM_COMPATIBLE_SUBROUTINES As int  = 0x00008E4A
 // Public Const COMPATIBLE_SUBROUTINES As int  = 0x00008E4B
 // Public Const PATCHES As int  = 0x0000000E
 // Public Const PATCH_VERTICES As int  = 0x00008E72
 // Public Const PATCH_DEFAULT_INNER_LEVEL As int  = 0x00008E73
 // Public Const PATCH_DEFAULT_OUTER_LEVEL As int  = 0x00008E74
 // Public Const TESS_CONTROL_OUTPUT_VERTICES As int  = 0x00008E75
 // Public Const TESS_GEN_MODE As int  = 0x00008E76
 // Public Const TESS_GEN_SPACING As int  = 0x00008E77
 // Public Const TESS_GEN_VERTEX_ORDER As int  = 0x00008E78
 // Public Const TESS_GEN_POINT_MODE As int  = 0x00008E79
 // Public Const ISOLINES As int  = 0x00008E7A
 // Public Const FRACTIONAL_ODD As int  = 0x00008E7B
 // Public Const FRACTIONAL_EVEN As int  = 0x00008E7C
 // Public Const MAX_PATCH_VERTICES As int  = 0x00008E7D
 // Public Const MAX_TESS_GEN_LEVEL As int  = 0x00008E7E
 // Public Const MAX_TESS_CONTROL_UNIFORM_COMPONENTS As int  = 0x00008E7F
 // Public Const MAX_TESS_EVALUATION_UNIFORM_COMPONENTS As int  = 0x00008E80
 // Public Const MAX_TESS_CONTROL_TEXTURE_IMAGE_UNITS As int  = 0x00008E81
 // Public Const MAX_TESS_EVALUATION_TEXTURE_IMAGE_UNITS As int  = 0x00008E82
 // Public Const MAX_TESS_CONTROL_OUTPUT_COMPONENTS As int  = 0x00008E83
 // Public Const MAX_TESS_PATCH_COMPONENTS As int  = 0x00008E84
 // Public Const MAX_TESS_CONTROL_TOTAL_OUTPUT_COMPONENTS As int  = 0x00008E85
 // Public Const MAX_TESS_EVALUATION_OUTPUT_COMPONENTS As int  = 0x00008E86
 // Public Const MAX_TESS_CONTROL_UNIFORM_BLOCKS As int  = 0x00008E89
 // Public Const MAX_TESS_EVALUATION_UNIFORM_BLOCKS As int  = 0x00008E8A
 // Public Const MAX_TESS_CONTROL_INPUT_COMPONENTS As int  = 0x0000886C
 // Public Const MAX_TESS_EVALUATION_INPUT_COMPONENTS As int  = 0x0000886D
 // Public Const MAX_COMBINED_TESS_CONTROL_UNIFORM_COMPONENTS As int  = 0x00008E1E
 // Public Const MAX_COMBINED_TESS_EVALUATION_UNIFORM_COMPONENTS As int  = 0x00008E1F
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_TESS_CONTROL_SHADER As int  = 0x000084F0
 // Public Const UNIFORM_BLOCK_REFERENCED_BY_TESS_EVALUATION_SHADER As int  = 0x000084F1
 // Public Const TESS_EVALUATION_SHADER As int  = 0x00008E87
 // Public Const TESS_CONTROL_SHADER As int  = 0x00008E88
 // Public Const TRANSFORM_FEEDBACK As int  = 0x00008E22
 // Public Const TRANSFORM_FEEDBACK_BUFFER_PAUSED As int  = 0x00008E23
 // Public Const TRANSFORM_FEEDBACK_BUFFER_ACTIVE As int  = 0x00008E24
 // Public Const TRANSFORM_FEEDBACK_BINDING As int  = 0x00008E25
 // Public Const MAX_TRANSFORM_FEEDBACK_BUFFERS As int  = 0x00008E70
 //
 // Public Const VERSION_3_3 1
 // Public Const VERTEX_ATTRIB_ARRAY_DIVISOR As int  = 0x000088FE
 // Public Const SRC1_COLOR As int  = 0x000088F9
 // Public Const ONE_MINUS_SRC1_COLOR As int  = 0x000088FA
 // Public Const ONE_MINUS_SRC1_ALPHA As int  = 0x000088FB
 // Public Const MAX_DUAL_SOURCE_DRAW_BUFFERS As int  = 0x000088FC
 // Public Const ANY_SAMPLES_PASSED As int  = 0x00008C2F
 // Public Const SAMPLER_BINDING As int  = 0x00008919
 // Public Const RGB10_A2UI As int  = 0x0000906F
 // Public Const TEXTURE_SWIZZLE_R As int  = 0x00008E42
 // Public Const TEXTURE_SWIZZLE_G As int  = 0x00008E43
 // Public Const TEXTURE_SWIZZLE_B As int  = 0x00008E44
 // Public Const TEXTURE_SWIZZLE_A As int  = 0x00008E45
 // Public Const TEXTURE_SWIZZLE_RGBA As int  = 0x00008E46
 // Public Const TIME_ELAPSED As int  = 0x000088BF
 // Public Const TIMESTAMP As int  = 0x00008E28
 // Public Const INT_2_10_10_10_REV As int  = 0x00008D9F
 // Public Const FIXED As int  = 0x0000140C
 // Public Const IMPLEMENTATION_COLOR_READ_TYPE As int  = 0x00008B9A
 // Public Const IMPLEMENTATION_COLOR_READ_FORMAT As int  = 0x00008B9B
 // Public Const LOW_single As int  = 0x00008DF0
 // Public Const MEDIUM_single As int  = 0x00008DF1
 // Public Const HIGH_single As int  = 0x00008DF2
 // Public Const LOW_INT As int  = 0x00008DF3
 // Public Const MEDIUM_INT As int  = 0x00008DF4
 // Public Const HIGH_INT As int  = 0x00008DF5
 // Public Const SHADER_COMPILER As int  = 0x00008DFA
 // Public Const SHADER_BINARY_FORMATS As int  = 0x00008DF8
 // Public Const NUM_SHADER_BINARY_FORMATS As int  = 0x00008DF9
 // Public Const MAX_VERTEX_UNIFORM_VECTORS As int  = 0x00008DFB
 // Public Const MAX_VARYING_VECTORS As int  = 0x00008DFC
 // Public Const MAX_FRAGMENT_UNIFORM_VECTORS As int  = 0x00008DFD
 // Public Const RGB565 As int  = 0x00008D62
 // Public Const PROGRAM_BINARY_RETRIEVABLE_HINT As int  = 0x00008257
 // Public Const PROGRAM_BINARY_LENGTH As int  = 0x00008741
 // Public Const NUM_PROGRAM_BINARY_FORMATS As int  = 0x000087FE
 // Public Const PROGRAM_BINARY_FORMATS As int  = 0x000087FF
 // Public Const VERTEX_SHADER_BIT As int  = 0x000000000001
 // Public Const FRAGMENT_SHADER_BIT As int  = 0x000000000002
 // Public Const GEOMETRY_SHADER_BIT As int  = 0x000000000004
 // Public Const TESS_CONTROL_SHADER_BIT As int  = 0x000000000008
 // Public Const TESS_EVALUATION_SHADER_BIT As int  = 0x000000000010
 // Public Const ALL_SHADER_BITS As int  = 0x0000FFFFFFFF
 // Public Const PROGRAM_SEPARABLE As int  = 0x00008258
 // Public Const ACTIVE_PROGRAM As int  = 0x00008259
 // Public Const PROGRAM_PIPELINE_BINDING As int  = 0x0000825A
 // Public Const MAX_VIEWPORTS As int  = 0x0000825B
 // Public Const VIEWPORT_SUBPIXEL_BITS As int  = 0x0000825C
 // Public Const VIEWPORT_BOUNDS_RANGE As int  = 0x0000825D
 // Public Const LAYER_PROVOKING_VERTEX As int  = 0x0000825E
 // Public Const VIEWPORT_INDEX_PROVOKING_VERTEX As int  = 0x0000825F
 // Public Const UNDEFINED_VERTEX As int  = 0x00008260
 // Public Const VERSION_4_5 1
 // Public Const CONTEXT_LOST As int  = 0x00000507
 // Public Const NEGATIVE_ONE_TO_ONE As int  = 0x0000935E
 // Public Const ZERO_TO_ONE As int  = 0x0000935F
 // Public Const CLIP_ORIGIN As int  = 0x0000935C
 // Public Const CLIP_DEPTH_MODE As int  = 0x0000935D
 // Public Const QUERY_WAIT_INVERTED As int  = 0x00008E17
 // Public Const QUERY_NO_WAIT_INVERTED As int  = 0x00008E18
 // Public Const QUERY_BY_REGION_WAIT_INVERTED As int  = 0x00008E19
 // Public Const QUERY_BY_REGION_NO_WAIT_INVERTED As int  = 0x00008E1A
 // Public Const MAX_CULL_DISTANCES As int  = 0x000082F9
 // Public Const MAX_COMBINED_CLIP_AND_CULL_DISTANCES As int  = 0x000082FA
 // Public Const TEXTURE_TARGET As int  = 0x00001006
 // Public Const QUERY_TARGET As int  = 0x000082EA
 // Public Const GUILTY_CONTEXT_RESET As int  = 0x00008253
 // Public Const INNOCENT_CONTEXT_RESET As int  = 0x00008254
 // Public Const UNKNOWN_CONTEXT_RESET As int  = 0x00008255
 // Public Const RESET_NOTIFICATION_STRATEGY As int  = 0x00008256
 // Public Const LOSE_CONTEXT_ON_RESET As int  = 0x00008252
 // Public Const NO_RESET_NOTIFICATION As int  = 0x00008261
 // Public Const CONTEXT_FLAG_ROBUST_ACCESS_BIT As int  = 0x000000000004
 // Public Const CONTEXT_RELEASE_BEHAVIOR As int  = 0x000082FB
 // Public Const CONTEXT_RELEASE_BEHAVIOR_FLUSH As int  = 0x000082FC
 // Public Const Console.WriteLine(_OUTPUT_SYNCHRONOUS_ARB As int  = 0x00008242
 // Public Const Console.WriteLine(_NEXT_LOGGED_MESSAGE_LENGTH_ARB As int  = 0x00008243
 // Public Const Console.WriteLine(_CALLBACK_FUNCTION_ARB As int  = 0x00008244
 // Public Const Console.WriteLine(_CALLBACK_USER_PARAM_ARB As int  = 0x00008245
 // Public Const Console.WriteLine(_SOURCE_API_ARB As int  = 0x00008246
 // Public Const Console.WriteLine(_SOURCE_WINDOW_SYSTEM_ARB As int  = 0x00008247
 // Public Const Console.WriteLine(_SOURCE_SHADER_COMPILER_ARB As int  = 0x00008248
 // Public Const Console.WriteLine(_SOURCE_THIRD_PARTY_ARB As int  = 0x00008249
 // Public Const Console.WriteLine(_SOURCE_APPLICATION_ARB As int  = 0x0000824A
 // Public Const Console.WriteLine(_SOURCE_OTHER_ARB As int  = 0x0000824B
 // Public Const Console.WriteLine(_TYPE_ERROR_ARB As int  = 0x0000824C
 // Public Const Console.WriteLine(_TYPE_DEPRECATED_BEHAVIOR_ARB As int  = 0x0000824D
 // Public Const Console.WriteLine(_TYPE_UNDEFINED_BEHAVIOR_ARB As int  = 0x0000824E
 // Public Const Console.WriteLine(_TYPE_PORTABILITY_ARB As int  = 0x0000824F
 // Public Const Console.WriteLine(_TYPE_PERFORMANCE_ARB As int  = 0x00008250
 // Public Const Console.WriteLine(_TYPE_OTHER_ARB As int  = 0x00008251
 // Public Const MAX_Console.WriteLine(_MESSAGE_LENGTH_ARB As int  = 0x00009143
 // Public Const MAX_Console.WriteLine(_LOGGED_MESSAGES_ARB As int  = 0x00009144
 // Public Const Console.WriteLine(_LOGGED_MESSAGES_ARB As int  = 0x00009145
 // Public Const Console.WriteLine(_SEVERITY_HIGH_ARB As int  = 0x00009146
 // Public Const Console.WriteLine(_SEVERITY_MEDIUM_ARB As int  = 0x00009147
 // Public Const Console.WriteLine(_SEVERITY_LOW_ARB As int  = 0x00009148
 // Public Const ARB_geometry_shader4 1
 // Public Const LINES_ADJACENCY_ARB As int  = 0x0000000A
 // Public Const LINE_STRIP_ADJACENCY_ARB As int  = 0x0000000B
 // Public Const TRIANGLES_ADJACENCY_ARB As int  = 0x0000000C
 // Public Const TRIANGLE_STRIP_ADJACENCY_ARB As int  = 0x0000000D
 // Public Const PROGRAM_POINT_SIZE_ARB As int  = 0x00008642
 // Public Const MAX_GEOMETRY_TEXTURE_IMAGE_UNITS_ARB As int  = 0x00008C29
 // Public Const FRAMEBUFFER_ATTACHMENT_LAYERED_ARB As int  = 0x00008DA7
 // Public Const FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS_ARB As int  = 0x00008DA8
 // Public Const FRAMEBUFFER_INCOMPLETE_LAYER_COUNT_ARB As int  = 0x00008DA9
 // Public Const GEOMETRY_SHADER_ARB As int  = 0x00008DD9
 // Public Const GEOMETRY_VERTICES_OUT_ARB As int  = 0x00008DDA
 // Public Const GEOMETRY_INPUT_TYPE_ARB As int  = 0x00008DDB
 // Public Const GEOMETRY_OUTPUT_TYPE_ARB As int  = 0x00008DDC
 // Public Const MAX_GEOMETRY_VARYING_COMPONENTS_ARB As int  = 0x00008DDD
 // Public Const MAX_VERTEX_VARYING_COMPONENTS_ARB As int  = 0x00008DDE
 // Public Const MAX_GEOMETRY_UNIFORM_COMPONENTS_ARB As int  = 0x00008DDF
 // Public Const MAX_GEOMETRY_OUTPUT_VERTICES_ARB As int  = 0x00008DE0
 // Public Const MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS_ARB As int  = 0x00008DE1
 //
 // Public Const ARB_gpu_shader_int64 1
 // Public Const INT64_ARB As int  = 0x0000140E
 // Public Const INT64_VEC2_ARB As int  = 0x00008FE9
 // Public Const INT64_VEC3_ARB As int  = 0x00008FEA
 // Public Const INT64_VEC4_ARB As int  = 0x00008FEB
 // Public Const UNSIGNED_INT64_VEC2_ARB As int  = 0x00008FF5
 // Public Const UNSIGNED_INT64_VEC3_ARB As int  = 0x00008FF6
 // Public Const UNSIGNED_INT64_VEC4_ARB As int  = 0x00008FF7
 // Public Const VERSION_4_4 1
 // Public Const MAX_VERTEX_ATTRIB_STRIDE As int  = 0x000082E5
 // Public Const PRIMITIVE_RESTART_FOR_PATCHES_SUPPORTED As int  = 0x00008221
 // Public Const TEXTURE_BUFFER_BINDING As int  = 0x00008C2A
 // Public Const MAP_PERSISTENT_BIT As int  = 0x00000040
 // Public Const MAP_COHERENT_BIT As int  = 0x00000080
 // Public Const DYNAMIC_STORAGE_BIT As int  = 0x00000100
 // Public Const CLIENT_STORAGE_BIT As int  = 0x00000200
 // Public Const CLIENT_MAPPED_BUFFER_BARRIER_BIT As int  = 0x000000004000
 // Public Const BUFFER_IMMUTABLE_STORAGE As int  = 0x0000821F
 // Public Const BUFFER_STORAGE_FLAGS As int  = 0x00008220
 // Public Const CLEAR_TEXTURE As int  = 0x00009365
 // Public Const LOCATION_COMPONENT As int  = 0x0000934A
 // Public Const TRANSFORM_FEEDBACK_BUFFER_INDEX As int  = 0x0000934B
 // Public Const TRANSFORM_FEEDBACK_BUFFER_STRIDE As int  = 0x0000934C
 // Public Const QUERY_BUFFER As int  = 0x00009192
 // Public Const QUERY_BUFFER_BARRIER_BIT As int  = 0x000000008000
 // Public Const QUERY_BUFFER_BINDING As int  = 0x00009193
 // Public Const QUERY_RESULT_NO_WAIT As int  = 0x00009194
 // Public Const MIRROR_CLAMP_TO_EDGE As int  = 0x00008743
 // Public Const VERSION_4_6 1
 // Public Const SHADER_BINARY_FORMAT_SPIR_V As int  = 0x00009551
 // Public Const SPIR_V_BINARY As int  = 0x00009552
 // Public Const PARAMETER_BUFFER As int  = 0x000080EE
 // Public Const PARAMETER_BUFFER_BINDING As int  = 0x000080EF
 // Public Const CONTEXT_FLAG_NO_ERROR_BIT As int  = 0x000000000008
 // Public Const VERTICES_SUBMITTED As int  = 0x000082EE
 // Public Const PRIMITIVES_SUBMITTED As int  = 0x000082EF
 // Public Const VERTEX_SHADER_INVOCATIONS As int  = 0x000082F0
 // Public Const TESS_CONTROL_SHADER_PATCHES As int  = 0x000082F1
 // Public Const TESS_EVALUATION_SHADER_INVOCATIONS As int  = 0x000082F2
 // Public Const GEOMETRY_SHADER_PRIMITIVES_EMITTED As int  = 0x000082F3
 // Public Const FRAGMENT_SHADER_INVOCATIONS As int  = 0x000082F4
 // Public Const COMPUTE_SHADER_INVOCATIONS As int  = 0x000082F5
 // Public Const CLIPPING_INPUT_PRIMITIVES As int  = 0x000082F6
 // Public Const CLIPPING_OUTPUT_PRIMITIVES As int  = 0x000082F7
 // Public Const POLYGON_OFFSET_CLAMP As int  = 0x00008E1B
 // Public Const SPIR_V_EXTENSIONS As int  = 0x00009553
 // Public Const NUM_SPIR_V_EXTENSIONS As int  = 0x00009554
 // Public Const TEXTURE_MAX_ANISOTROPY As int  = 0x000084FE
 // Public Const MAX_TEXTURE_MAX_ANISOTROPY As int  = 0x000084FF
 // Public Const TRANSFORM_FEEDBACK_OVERFLOW As int  = 0x000082EC
 // Public Const TRANSFORM_FEEDBACK_STREAM_OVERFLOW As int  = 0x000082ED
 // Public Const ARB_compute_variable_group_size 1
 // Public Const MAX_COMPUTE_VARIABLE_GROUP_INVOCATIONS_ARB As int  = 0x00009344
 // Public Const MAX_COMPUTE_FIXED_GROUP_INVOCATIONS_ARB As int  = 0x000090EB
 // Public Const MAX_COMPUTE_VARIABLE_GROUP_SIZE_ARB As int  = 0x00009345
 // Public Const MAX_COMPUTE_FIXED_GROUP_SIZE_ARB As int  = 0x000091BF

// public static void glCullFace(int Mode)
//     {

// public static void glFrontFace(int Mode)
//     {

// public static void glHint(int iTarget, int Mode)
//     {

// public static void glLineWIdth(float fWIdth)
//     {

// public static void glPointSize(float fSize)
//     {

// public static void glPolygonMode(int face, int Mode)
//     {

// public static void glScissor(int x, int y, int iWIdth, int iHeight)
//     {

// public static void glTexParameterf(int iTarget, int iPname, float fParam)
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

// public static void glClearColor(float red, float green, float blue, float alpha)
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

// public static void glPixelStoref(int iPname, float fParam)
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

// public static void glGetsinglev(int iPname, float[] data)
//     {

// public static void glGetintv(int iPname, int[] data)
//     {

// public static string glGetString(int name)
//     {

// public static void glGetTexImage(int iTarget, int iLevel, int iFormat, int iType, Byte pixels)
//     {

// public static void glGetTexParameterfv(int iTarget, int iPname, float[] params)
//     {

// public static void glGetTexParameteriv(int iTarget, int iPname, int[] params)
//     {

// public static void glGetTexLevelParameterfv(int iTarget, int iLevel, int iPname, float[] params)
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

// public static void glPolygonOffset(float factor, float units)
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

// public static void glSampleCoverage(float value, Byte invert)
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

// public static void glPointParameterf(int iPname, float fParam)
//     {

// public static void glPointParameterfv(int iPname, float[] flxParams)
//     {

// public static void glPointParameteri(int iPname, int iParam)
//     {

// public static void glPointParameteriv(int iPname, int[] inxParams)
//     {

// public static void glBlendColor(float red, float green, float blue, float alpha)
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

// public static void glGetUniformfv(int program, int location, float[] params)
//     {

// public static void glGetUniformiv(int program, int location, int[] params)
//     {

// public static void glGetVertexAttribdv(int iIndex, int iPname, double[] params)
//     {

// public static void glGetVertexAttribfv(int iIndex, int iPname, float[] params)
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

// public static void glUniform1f(int location, float v0)
//     {

// public static void glUniform2f(int location, float v0, float v1)
//     {

// public static void glUniform3f(int location, float v0, float v1, float v2)
//     {

// public static void glUniform4f(int location, float v0, float v1, float v2, float v3)
//     {

// public static void glUniform1i(int location, int v0)
//     {

// public static void glUniform2i(int location, int v0, int v1)
//     {

// public static void glUniform3i(int location, int v0, int v1, int v2)
//     {

// public static void glUniform4i(int location, int v0, int v1, int v2, int v3)
//     {

// public static void glUniform1fv(int location, int iSize, float[] value)
//     {

// public static void glUniform2fv(int location, int iSize, float[] value)
//     {

// public static void glUniform3fv(int location, int iSize, float[] value)
//     {

// public static void glUniform4fv(int location, int iSize, float[] value)
//     {

// public static void glUniform1iv(int location, int iSize, int[] value)
//     {

// public static void glUniform2iv(int location, int iSize, int[] value)
//     {

// public static void glUniform3iv(int location, int iSize, int[] value)
//     {

// public static void glUniform4iv(int location, int iSize, int[] value)
//     {

//  // Public Extern glUniformMatrix2fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
//  // Public Extern glUniformMatrix3fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
//  // Public Extern glUniformMatrix4fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
// public static void glValIdateProgram(int program)
//     {

 // Public Extern glVertexAttrib1d(iIndex As int, x As Float)
 // Public Extern glVertexAttrib1dv(iIndex As int, const As Float * v)
 // Public Extern glVertexAttrib1f(iIndex As int, GLsingle x)
 // Public Extern glVertexAttrib1fv(iIndex As int, const GLsingle * v)
 // Public Extern glVertexAttrib1s(iIndex As int, GLshort x)
 // Public Extern glVertexAttrib1sv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttrib2d(iIndex As int, x As Float, As Float y)
 // Public Extern glVertexAttrib2dv(iIndex As int, const As Float * v)
 // Public Extern glVertexAttrib2f(iIndex As int, GLsingle x, GLsingle y)
 // Public Extern glVertexAttrib2fv(iIndex As int, const GLsingle * v)
 // Public Extern glVertexAttrib2s(iIndex As int, GLshort x, GLshort y)
 // Public Extern glVertexAttrib2sv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttrib3d(iIndex As int, x As Float, As Float y, As Float z)
 // Public Extern glVertexAttrib3dv(iIndex As int, const As Float * v)
 // Public Extern glVertexAttrib3f(iIndex As int, GLsingle x, GLsingle y, GLsingle z)
 // Public Extern glVertexAttrib3fv(iIndex As int, const GLsingle * v)
 // Public Extern glVertexAttrib3s(iIndex As int, GLshort x, GLshort y, GLshort z)
 // Public Extern glVertexAttrib3sv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttrib4Nbv(iIndex As int, const GLbyte * v)
 // Public Extern glVertexAttrib4Niv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttrib4Nsv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttrib4Nub(iIndex As int, GLubyte x, GLubyte y, GLubyte z, GLubyte w)
 // Public Extern glVertexAttrib4Nubv(iIndex As int, const GLubyte * v)
 // Public Extern glVertexAttrib4Nuiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttrib4Nusv(iIndex As int, const GLushort * v)
 // Public Extern glVertexAttrib4bv(iIndex As int, const GLbyte * v)
 // Public Extern glVertexAttrib4d(iIndex As int, x As Float, As Float y, As Float z, As Float w)
 // Public Extern glVertexAttrib4dv(iIndex As int, const As Float * v)
 // Public Extern glVertexAttrib4f(iIndex As int, GLsingle x, GLsingle y, GLsingle z, GLsingle w)
 // Public Extern glVertexAttrib4fv(iIndex As int, const GLsingle * v)
 // Public Extern glVertexAttrib4iv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttrib4s(iIndex As int, GLshort x, GLshort y, GLshort z, GLshort w)
 // Public Extern glVertexAttrib4sv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttrib4ubv(iIndex As int, const GLubyte * v)
 // Public Extern glVertexAttrib4uiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttrib4usv(iIndex As int, const GLushort * v)
 // Public Extern glVertexAttribPointer(iIndex As int, size As int, iType As int, normalized As Boolean, stride As int, ipointer As Byte[]) // FIXME: verificar

 // Public Extern glUniformMatrix2x3fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
 // Public Extern glUniformMatrix3x2fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
 // Public Extern glUniformMatrix2x4fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
 // Public Extern glUniformMatrix4x2fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
 // Public Extern glUniformMatrix3x4fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)
 // Public Extern glUniformMatrix4x3fv(location As int, iSize As int, As Byte transpose, const GLsingle * value)

 // Public Extern glColorMaski(iIndex As int, as byte r, as byte g, as byte b, as byte a)
 // Public Extern glGetBooleani_v(iTarget As int, iIndex As int, as byte data as byte[])
 // Public Extern glGetinti_v(iTarget As int, iIndex As int, GLint data as byte[])
 // Public Extern glEnablei(iTarget As int, iIndex As int)
 // Public Extern glDisablei(iTarget As int, iIndex As int)
 // GLAPI as byte APIENTRY glIsEnabledi(iTarget As int, iIndex As int)
 // Public Extern glBeginTransformFeedback(as int primitiveMode)
 // Public Extern glEndTransformFeedback()
 // Public Extern glBindBufferRange(iTarget As int, iIndex As int, buffer as int, offset as int, size as int)
 // Public Extern glBindBufferBase(iTarget As int, iIndex As int, buffer as int)
 // Public Extern glTransformFeedbackVaryings(program As int, iSize as int, const GLchar * const * varyings, iBuff as intferMode)
 // Public Extern glGetTransformFeedbackVarying(program As int, iIndex As int, BufSize As int, legth as int[], GLsizei * size, as int * type, name as string)
 // Public Extern glClampColor(iTarget As int, as int clamp)
 // Public Extern glBeginConditionalRender(id as int, Mode As int)
 // Public Extern glEndConditionalRender()
 // Public Extern glVertexAttribIPointer(iIndex As int, size as int, iType As int, stride as int, const * pointer)
 // Public Extern glGetVertexAttribIiv(iIndex As int, iPname As int, params as int[])
 // Public Extern glGetVertexAttribIuiv(iIndex As int, iPname As int, params as int[])
 // Public Extern glVertexAttribI1i(iIndex As int, x As int)
 // Public Extern glVertexAttribI2i(iIndex As int, x As int, y As int)
 // Public Extern glVertexAttribI3i(iIndex As int, x As int, y As int, GLint z)
 // Public Extern glVertexAttribI4i(iIndex As int, x As int, y As int, GLint z, GLint w)
 // Public Extern glVertexAttribI1ui(iIndex As int, GLuint x)
 // Public Extern glVertexAttribI2ui(iIndex As int, GLuint x, GLuint y)
 // Public Extern glVertexAttribI3ui(iIndex As int, GLuint x, GLuint y, GLuint z)
 // Public Extern glVertexAttribI4ui(iIndex As int, GLuint x, GLuint y, GLuint z, GLuint w)
 // Public Extern glVertexAttribI1iv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttribI2iv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttribI3iv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttribI4iv(iIndex As int, const GLint * v)
 // Public Extern glVertexAttribI1uiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttribI2uiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttribI3uiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttribI4uiv(iIndex As int, const GLuint * v)
 // Public Extern glVertexAttribI4bv(iIndex As int, const GLbyte * v)
 // Public Extern glVertexAttribI4sv(iIndex As int, const GLshort * v)
 // Public Extern glVertexAttribI4ubv(iIndex As int, const GLubyte * v)
 // Public Extern glVertexAttribI4usv(iIndex As int, const GLushort * v)
 // Public Extern glGetUniformuiv(program As int, location as int, params as int[])
 // Public Extern glBindFragDataLocation(program As int, GLuint color, const name as string)
 // Public Extern Function glGetFragDataLocation(program As int, const name as string)
 // Public Extern glUniform1ui(location as int, GLuint v0)
 // Public Extern glUniform2ui(location as int, GLuint v0, GLuint v1)
 // Public Extern glUniform3ui(location as int, GLuint v0, GLuint v1, GLuint v2)
 // Public Extern glUniform4ui(location as int, GLuint v0, GLuint v1, GLuint v2, GLuint v3)
 // Public Extern glUniform1uiv(location as int, iSize as int, const GLuint * value)
 // Public Extern glUniform2uiv(location as int, iSize as int, const GLuint * value)
 // Public Extern glUniform3uiv(location as int, iSize as int, const GLuint * value)
 // Public Extern glUniform4uiv(location as int, iSize as int, const GLuint * value)
 // Public Extern glTexParameterIiv(iTarget As int, iPname As int, inxParams as int[])
 // Public Extern glTexParameterIuiv(iTarget As int, iPname As int, const params as int[])
 // Public Extern glGetTexParameterIiv(iTarget As int, iPname As int, params as int[])
 // Public Extern glGetTexParameterIuiv(iTarget As int, iPname As int, params as int[])
 // Public Extern glClearBufferiv(iBuff as intfer, GLint drawbuffer, value as int[])
 // Public Extern glClearBufferuiv(iBuff as intfer, GLint drawbuffer, const GLuint * value)
 // Public Extern glClearBufferfv(iBuff as intfer, GLint drawbuffer, const GLsingle * value)
 // Public Extern glClearBufferfi(iBuff as intfer, GLint drawbuffer, GLsingle depth, GLint stencil)
 // GLAPI const GLubyte * APIENTRY glGetStringi(as int name, iIndex As int)
 // GLAPI as byte APIENTRY glIsRenderbuffer(GLuint renderbuffer)
 // Public Extern glBindRenderbuffer(iTarget As int, GLuint renderbuffer)
 // Public Extern glDeleteRenderbuffers(n as int, const GLuint * renderbuffers)
 // Public Extern glGenRenderbuffers(n as int, GLuint * renderbuffers)
 // Public Extern glRenderbufferStorage(iTarget As int,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glGetRenderbufferParameteriv(iTarget As int, iPname As int, params as int[])
 // GLAPI as byte APIENTRY glIsFramebuffer(GLuint framebuffer)
 // Public Extern glBindFramebuffer(iTarget As int, GLuint framebuffer)
 // Public Extern glDeleteFramebuffers(n as int, const GLuint * framebuffers)
 // Public Extern glGenFramebuffers(n as int, GLuint * framebuffers)
 // GLAPI as int APIENTRY glCheckFramebufferStatus(iTarget As int)
 // Public Extern glFramebufferTexture1D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int)
 // Public Extern glFramebufferTexture2D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int)
 // Public Extern glFramebufferTexture3D(iTarget As int, as int attachment, as int textarget, texture as int, iLevel as int, zOffset as int)
 // Public Extern glFramebufferRenderbuffer(iTarget As int, as int attachment, as int renderbuffertarget, GLuint renderbuffer)
 // Public Extern glGetFramebufferAttachmentParameteriv(iTarget As int, as int attachment, iPname As int, params as int[])
 // Public Extern glGenerateMipmap(iTarget As int)
 // Public Extern glBlitFramebuffer(GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GiMaskBitField as int, as int filter)
 // Public Extern glRenderbufferStorageMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glFramebufferTextureLayer(iTarget As int, as int attachment, texture as int, iLevel as int, GLint layer)
 // GLAPI * APIENTRY glMapBufferRange(iTarget As int, offset as int, GLsizeiptr length, GLbitfield access)
 // Public Extern glFlushMappedBufferRange(iTarget As int, offset as int, GLsizeiptr length)
 // Public Extern glBindVertexArray(GLuint array)
 // Public Extern glDeleteVertexArrays(n as int, const GLuint * arrays)
 // Public Extern glGenVertexArrays(n as int, GLuint * arrays)
 // GLAPI as byte APIENTRY glIsVertexArray(GLuint array)

 // Public Extern glDrawArraysInstanced(Mode As int, first as int, iSize as int, GLsizei instancecount)
 // Public Extern glDrawElementsInstanced(Mode As int, iSize as int, iType As int, const * indices, GLsizei instancecount)
 // Public Extern glTexBuffer(iTarget As int,internalformat as int, buffer as int)
 // Public Extern glPrimitiveRestartIndex(iIndex As int)
 // Public Extern glCopyBufferSubData(as int readTarget, as int writeTarget, GLintptr readOffset, GLintptr writeOffset, size as int)
 // Public Extern glGetUniformIndices(program As int, GLsizei uniformCount, const GLchar * const * uniformNames, GLuint * uniformIndices)
 // Public Extern glGetActiveUniformsiv(program As int, GLsizei uniformCount, const GLuint * uniformIndices, iPname As int, params as int[])
 // Public Extern glGetActiveUniformName(program As int, GLuint uniformIndex, BufSize As int, legth as int[], GLchar * uniformName)
 // GLAPI GLuint APIENTRY glGetUniformBlockIndex(program As int, const GLchar * uniformBlockName)
 // Public Extern glGetActiveUniformBlockiv(program As int, GLuint uniformBlockIndex, iPname As int, params as int[])
 // Public Extern glGetActiveUniformBlockName(program As int, GLuint uniformBlockIndex, BufSize As int, legth as int[], GLchar * uniformBlockName)
 // Public Extern glUniformBlockBinding(program As int, GLuint uniformBlockIndex, GLuint uniformBlockBinding)
 //

 // Public Extern glDrawElementsBaseVertex(Mode As int, iSize as int, iType As int, const * indices, GLint basevertex)
 // Public Extern glDrawRangeElementsBaseVertex(Mode As int, start as int, iEnd as int , iSize as int, iType As int, const * indices, GLint basevertex)
 // Public Extern glDrawElementsInstancedBaseVertex(Mode As int, iSize as int, iType As int, const * indices, GLsizei instancecount, GLint basevertex)
 // Public Extern glMultiDrawElementsBaseVertex(Mode As int, const GLsizei * count, iType As int, const * const * indices, drawcount as int, const GLint * basevertex)
 // Public Extern glProvokingVertex(Mode As int)
 // GLAPI GLsync APIENTRY glFenceSync(as int condition, GLbitfield flags)
 // GLAPI as byte APIENTRY glIsSync(GLsync sync)
 // Public Extern glDeleteSync(GLsync sync)
 // GLAPI as int APIENTRY glClientWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)
 // Public Extern glWaitSync(GLsync sync, GLbitfield flags, GLuint64 timeout)
 // Public Extern glGetint64v(iPname As int, GLint64 data as byte[])
 // Public Extern glGetSynciv(GLsync sync, iPname As int, BufSize As int, legth as int[], GLint * values)
 // Public Extern glGetint64i_v(iTarget As int, iIndex As int, GLint64 data as byte[])
 // Public Extern glGetBufferParameteri64v(iTarget As int, iPname As int, GLint64 * params)
 // Public Extern glFramebufferTexture(iTarget As int, as int attachment, texture as int, iLevel as int)
 // Public Extern glTexImage2DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // Public Extern glTexImage3DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // Public Extern glGetMultisamplefv(iPname As int, iIndex As int, GLsingle * val)
 // Public Extern glSampleMaski(mask as intNumber, GiMaskBitField as int)

 // Public Extern glBindFragDataLocationIndexed(program As int, GLuint colorNumber, iIndex As int, const name as string)
 // Public Extern Function glGetFragDataIndex(program As int, const name as string)
 // Public Extern glGenSamplers(iSize as int, GLuint * samplers)
 // Public Extern glDeleteSamplers(iSize as int, const GLuint * samplers)
 // GLAPI as byte APIENTRY glIsSampler(GLuint sampler)
 // Public Extern glBindSampler(GLuint unit, GLuint sampler)
 // Public Extern glSamplerParameteri(GLuint sampler, iPname As int, iParam As int)
 // Public Extern glSamplerParameteriv(GLuint sampler, iPname As int, const GLint * param)
 // Public Extern glSamplerParameterf(GLuint sampler, iPname As int, fParam As single)
 // Public Extern glSamplerParameterfv(GLuint sampler, iPname As int, const GLsingle * param)
 // Public Extern glSamplerParameterIiv(GLuint sampler, iPname As int, const GLint * param)
 // Public Extern glSamplerParameterIuiv(GLuint sampler, iPname As int, const GLuint * param)
 // Public Extern glGetSamplerParameteriv(GLuint sampler, iPname As int, params as int[])
 // Public Extern glGetSamplerParameterIiv(GLuint sampler, iPname As int, params as int[])
 // Public Extern glGetSamplerParameterfv(GLuint sampler, iPname As int, params as single[])
 // Public Extern glGetSamplerParameterIuiv(GLuint sampler, iPname As int, params as int[])
 // Public Extern glQueryCounter(id as int, iTarget As int)
 // Public Extern glGetQueryObjecti64v(id as int, iPname As int, GLint64 * params)
 // Public Extern glGetQueryObjectui64v(id as int, iPname As int, GLuint64 * params)
 // Public Extern glVertexAttribDivisor(iIndex As int, GLuint divisor)
 // Public Extern glVertexAttribP1ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // Public Extern glVertexAttribP1uiv(iIndex As int, iType As int, as byte normalized, const GLuint * value)
 // Public Extern glVertexAttribP2ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // Public Extern glVertexAttribP2uiv(iIndex As int, iType As int, as byte normalized, const GLuint * value)
 // Public Extern glVertexAttribP3ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // Public Extern glVertexAttribP3uiv(iIndex As int, iType As int, as byte normalized, const GLuint * value)
 // Public Extern glVertexAttribP4ui(iIndex As int, iType As int, as byte normalized, GLuint value)
 // Public Extern glVertexAttribP4uiv(iIndex As int, iType As int, as byte normalized, const GLuint * value)
 //

 // Public Extern glMinSampleShading(value as single)
 // Public Extern glBlendEquationi(GLuint buf, Mode As int)
 // Public Extern glBlendEquationSeparatei(GLuint buf, Mode As intRGB, Mode As intAlpha)
 // Public Extern glBlendFunci(GLuint buf, as int src, as int dst)
 // Public Extern glBlendFuncSeparatei(GLuint buf, as int srcRGB, as int dstRGB, as int srcAlpha, as int dstAlpha)
 // Public Extern glDrawArraysIndirect(Mode As int, const * indirect)
 // Public Extern glDrawElementsIndirect(Mode As int, iType As int, const * indirect)
 // Public Extern glUniform1d(location as int, x as float)
 // Public Extern glUniform2d(location as int, x as float, as float y)
 // Public Extern glUniform3d(location as int, x as float, as float y, as float z)
 // Public Extern glUniform4d(location as int, x as float, as float y, as float z, as float w)
 // Public Extern glUniform1dv(location as int, iSize as int, const as float * value)
 // Public Extern glUniform2dv(location as int, iSize as int, const as float * value)
 // Public Extern glUniform3dv(location as int, iSize as int, const as float * value)
 // Public Extern glUniform4dv(location as int, iSize as int, const as float * value)
 // Public Extern glUniformMatrix2dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix3dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix4dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix2x3dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix2x4dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix3x2dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix3x4dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix4x2dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glUniformMatrix4x3dv(location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glGetUniformdv(program As int, location as int, as float * params)
 // Public Extern Function glGetSubroutineUniformLocation(program As int, as int shadertype, const name as string)
 // GLAPI GLuint APIENTRY glGetSubroutineIndex(program As int, as int shadertype, const name as string)
 // Public Extern glGetActiveSubroutineUniformiv(program As int, as int shadertype, iIndex As int, iPname As int, GLint * values)
 // Public Extern glGetActiveSubroutineUniformName(program As int, as int shadertype, iIndex As int, BufSize As int, legth as int[], name as string)
 // Public Extern glGetActiveSubroutineName(program As int, as int shadertype, iIndex As int, BufSize As int, legth as int[], name as string)
 // Public Extern glUniformSubroutinesuiv(as int shadertype, iSize as int, const GLuint * indices)
 // Public Extern glGetUniformSubroutineuiv(as int shadertype, location as int, params as int[])
 // Public Extern glGetProgramStageiv(program As int, as int shadertype, iPname As int, GLint * values)
 // Public Extern glPatchParameteri(iPname As int, GLint value)
 // Public Extern glPatchParameterfv(iPname As int, const GLsingle * values)
 // Public Extern glBindTransformFeedback(iTarget As int, id as int)
 // Public Extern glDeleteTransformFeedbacks(n as int, ids as int[])
 // Public Extern glGenTransformFeedbacks(n as int, GLuint * ids)
 // GLAPI as byte APIENTRY glIsTransformFeedback(id as int)
 // Public Extern glPauseTransformFeedback()
 // Public Extern glResumeTransformFeedback()
 // Public Extern glDrawTransformFeedback(Mode As int, id as int)
 // Public Extern glDrawTransformFeedbackStream(Mode As int, id as int, GLuint stream)
 // Public Extern glBeginQueryIndexed(iTarget As int, iIndex As int, id as int)
 // Public Extern glEndQueryIndexed(iTarget As int, iIndex As int)
 // Public Extern glGetQueryIndexediv(iTarget As int, iIndex As int, iPname As int, params as int[])
 //

 // Public Extern glReleaseShaderCompiler()
 // Public Extern glShaderBinary(iSize as int, const GLuint * shaders, as int binaryformat, const * Binary , GLsizei length)
 // Public Extern glGetShaderPrecisionFormat(as int shadertype, as int precisiontype, GLint * range, GLint * precision)
 // Public Extern glDepthRangef(GLsingle n, GLsingle f)
 // Public Extern glClearDepthf(GLsingle d)
 // Public Extern glGetProgramBinary(program As int, BufSize As int, legth as int[], as int * binaryFormat, * Binary )
 // Public Extern glProgramBinary(program As int, as int binaryFormat, const * Binary , GLsizei length)
 // Public Extern glProgramParameteri(program As int, iPname As int, GLint value)
 // Public Extern glUseProgramStages(GLuint pipeline, GLbitfield stages, program As int)
 // Public Extern glActiveShaderProgram(GLuint pipeline, program As int)
 // GLAPI GLuint APIENTRY glCreateShaderProgramv(iType As int, iSize as int, const GLchar * const * strings)
 // Public Extern glBindProgramPipeline(GLuint pipeline)
 // Public Extern glDeleteProgramPipelines(n as int, const GLuint * pipelines)
 // Public Extern glGenProgramPipelines(n as int, GLuint * pipelines)
 // GLAPI as byte APIENTRY glIsProgramPipeline(GLuint pipeline)
 // Public Extern glGetProgramPipelineiv(GLuint pipeline, iPname As int, params as int[])
 // Public Extern glProgramUniform1i(program As int, location as int, v0 as int)
 // Public Extern glProgramUniform1iv(program As int, location as int, iSize as int, value as int[])
 // Public Extern glProgramUniform1f(program As int, location as int, v0 as single)
 // Public Extern glProgramUniform1fv(program As int, location as int, iSize as int, const GLsingle * value)
 // Public Extern glProgramUniform1d(program As int, location as int, as float v0)
 // Public Extern glProgramUniform1dv(program As int, location as int, iSize as int, const as float * value)
 // Public Extern glProgramUniform1ui(program As int, location as int, GLuint v0)
 // Public Extern glProgramUniform1uiv(program As int, location as int, iSize as int, const GLuint * value)
 // Public Extern glProgramUniform2i(program As int, location as int, v0 as int, v1 as int)
 // Public Extern glProgramUniform2iv(program As int, location as int, iSize as int, value as int[])
 // Public Extern glProgramUniform2f(program As int, location as int, v0 as single, v1 as single)
 // Public Extern glProgramUniform2fv(program As int, location as int, iSize as int, const GLsingle * value)
 // Public Extern glProgramUniform2d(program As int, location as int, as float v0, as float v1)
 // Public Extern glProgramUniform2dv(program As int, location as int, iSize as int, const as float * value)
 // Public Extern glProgramUniform2ui(program As int, location as int, GLuint v0, GLuint v1)
 // Public Extern glProgramUniform2uiv(program As int, location as int, iSize as int, const GLuint * value)
 // Public Extern glProgramUniform3i(program As int, location as int, v0 as int, v1 as int, v2 as int)
 // Public Extern glProgramUniform3iv(program As int, location as int, iSize as int, value as int[])
 // Public Extern glProgramUniform3f(program As int, location as int, v0 as single, v1 as single, v2 as single)
 // Public Extern glProgramUniform3fv(program As int, location as int, iSize as int, const GLsingle * value)
 // Public Extern glProgramUniform3d(program As int, location as int, as float v0, as float v1, as float v2)
 // Public Extern glProgramUniform3dv(program As int, location as int, iSize as int, const as float * value)
 // Public Extern glProgramUniform3ui(program As int, location as int, GLuint v0, GLuint v1, GLuint v2)
 // Public Extern glProgramUniform3uiv(program As int, location as int, iSize as int, const GLuint * value)
 // Public Extern glProgramUniform4i(program As int, location as int, v0 as int, v1 as int, v2 as int, v3 as int)
 // Public Extern glProgramUniform4iv(program As int, location as int, iSize as int, value as int[])
 // Public Extern glProgramUniform4f(program As int, location as int, v0 as single, v1 as single, v2 as single, v3 as single)
 // Public Extern glProgramUniform4fv(program As int, location as int, iSize as int, const GLsingle * value)
 // Public Extern glProgramUniform4d(program As int, location as int, as float v0, as float v1, as float v2, as float v3)
 // Public Extern glProgramUniform4dv(program As int, location as int, iSize as int, const as float * value)
 // Public Extern glProgramUniform4ui(program As int, location as int, GLuint v0, GLuint v1, GLuint v2, GLuint v3)
 // Public Extern glProgramUniform4uiv(program As int, location as int, iSize as int, const GLuint * value)
 // Public Extern glProgramUniformMatrix2fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix3fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix4fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix2dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix3dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix4dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix2x3fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix3x2fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix2x4fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix4x2fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix3x4fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix4x3fv(program As int, location as int, iSize as int, as byte transpose, const GLsingle * value)
 // Public Extern glProgramUniformMatrix2x3dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix3x2dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix2x4dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix4x2dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix3x4dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glProgramUniformMatrix4x3dv(program As int, location as int, iSize as int, as byte transpose, const as float * value)
 // Public Extern glValidateProgramPipeline(GLuint pipeline)
 // Public Extern glGetProgramPipelineInfoLog(GLuint pipeline, BufSize As int, legth as int[], GLchar * infoLog)
 // Public Extern glVertexAttribL1d(iIndex As int, x as float)
 // Public Extern glVertexAttribL2d(iIndex As int, x as float, as float y)
 // Public Extern glVertexAttribL3d(iIndex As int, x as float, as float y, as float z)
 // Public Extern glVertexAttribL4d(iIndex As int, x as float, as float y, as float z, as float w)
 // Public Extern glVertexAttribL1dv(iIndex As int, const as float * v)
 // Public Extern glVertexAttribL2dv(iIndex As int, const as float * v)
 // Public Extern glVertexAttribL3dv(iIndex As int, const as float * v)
 // Public Extern glVertexAttribL4dv(iIndex As int, const as float * v)
 // Public Extern glVertexAttribLPointer(iIndex As int, size as int, iType As int, stride as int, const * pointer)
 // Public Extern glGetVertexAttribLdv(iIndex As int, iPname As int, as float * params)
 // Public Extern glViewportArrayv(GLuint first, iSize as int, const GLsingle * v)
 // Public Extern glViewportIndexedf(iIndex As int, GLsingle x, GLsingle y, GLsingle w, GLsingle h)
 // Public Extern glViewportIndexedfv(iIndex As int, const GLsingle * v)
 // Public Extern glScissorArrayv(GLuint first, iSize as int, const GLint * v)
 // Public Extern glScissorIndexed(iIndex As int, GLint left, GLint bottom, iWidth As int, iHeight As int)
 // Public Extern glScissorIndexedv(iIndex As int, const GLint * v)
 // Public Extern glDepthRangeArrayv(GLuint first, iSize as int, const as float * v)
 // Public Extern glDepthRangeIndexed(iIndex As int, as float n, as float f)
 // Public Extern glGetsinglei_v(iTarget As int, iIndex As int, GLsingle data as byte[])
 // Public Extern glGetDoublei_v(iTarget As int, iIndex As int, as float data as byte[])
 //

 // Public Extern glDrawArraysInstancedBaseInstance(Mode As int, first as int, iSize as int, GLsizei instancecount, GLuint baseinstance)
 // Public Extern glDrawElementsInstancedBaseInstance(Mode As int, iSize as int, iType As int, const * indices, GLsizei instancecount, GLuint baseinstance)
 // Public Extern glDrawElementsInstancedBaseVertexBaseInstance(Mode As int, iSize as int, iType As int, const * indices, GLsizei instancecount, GLint basevertex, GLuint baseinstance)
 // Public Extern glGetInternalformativ(iTarget As int,internalformat as int, iPname As int, BufSize As int, params as int[])
 // Public Extern glGetActiveAtomicCounterBufferiv(program As int, buffer as intIndex, iPname As int, params as int[])
 // Public Extern glBindImageTexture(GLuint unit, texture as int, iLevel as int, as byte layered, GLint layer, as int access, iFormat as int)
 // Public Extern glMemoryBarrier(GLbitfield barriers)
 // Public Extern glTexStorage1D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int)
 // Public Extern glTexStorage2D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glTexStorage3D(iTarget As int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int, iDepth as int)
 // Public Extern glDrawTransformFeedbackInstanced(Mode As int, id as int, GLsizei instancecount)
 // Public Extern glDrawTransformFeedbackStreamInstanced(Mode As int, id as int, GLuint stream, GLsizei instancecount)

 // Public Extern glClearBufferData(iTarget As int,internalformat as int, iFormat as int, iType As int, data as byte[])
 // Public Extern glClearBufferSubData(iTarget As int,internalformat as int, offset as int, size as int, iFormat as int, iType As int, data as byte[])
 // Public Extern glDispatchCompute(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z)
 // Public Extern glDispatchComputeIndirect(GLintptr indirect)
 // Public Extern glCopyImageSubData(GLuint srcName, as int srcTarget, GLint srcLevel, GLint srcX, GLint srcY, GLint srcZ, GLuint dstName, as int dstTarget, GLint dstLevel, GLint dstX, GLint dstY, GLint dstZ, GLsizei srcWidth, GLsizei srcHeight, GLsizei srcDepth)
 // Public Extern glFramebufferParameteri(iTarget As int, iPname As int, iParam As int)
 // Public Extern glGetFramebufferParameteriv(iTarget As int, iPname As int, params as int[])
 // Public Extern glGetInternalformati64v(iTarget As int,internalformat as int, iPname As int, BufSize As int, GLint64 * params)
 // Public Extern glInvalidateTexSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int)
 // Public Extern glInvalidateTexImage(texture as int, iLevel as int)
 // Public Extern glInvalidateBufferSubData(buffer as int, offset as int, GLsizeiptr length)
 // Public Extern glInvalidateBufferData(buffer as int)
 // Public Extern glInvalidateFramebuffer(iTarget As int, n as intumAttachments, const as int * attachments)
 // Public Extern glInvalidateSubFramebuffer(iTarget As int, n as intumAttachments, const as int * attachments, x As int, y As int, iWidth As int, iHeight As int)
 // Public Extern glMultiDrawArraysIndirect(Mode As int, const * indirect, drawcount as int, stride as int)
 // Public Extern glMultiDrawElementsIndirect(Mode As int, iType As int, const * indirect, drawcount as int, stride as int)
 // Public Extern glGetProgramInterfaceiv(program As int, as int programInterface, iPname As int, params as int[])
 // GLAPI GLuint APIENTRY glGetProgramResourceIndex(program As int, as int programInterface, const name as string)
 // Public Extern glGetProgramResourceName(program As int, as int programInterface, iIndex As int, BufSize As int, legth as int[], name as string)
 // Public Extern glGetProgramResourceiv(program As int, as int programInterface, iIndex As int, GLsizei propCount, const as int * props, BufSize As int, legth as int[], params as int[])
 // Public Extern Function glGetProgramResourceLocation(program As int, as int programInterface, const name as string)
 // Public Extern Function glGetProgramResourceLocationIndex(program As int, as int programInterface, const name as string)
 // Public Extern glShaderStorageBlockBinding(program As int, GLuint storageBlockIndex, GLuint storageBlockBinding)
 // Public Extern glTexBufferRange(iTarget As int,internalformat as int, buffer as int, offset as int, size as int)
 // Public Extern glTexStorage2DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // Public Extern glTexStorage3DMultisample(iTarget As int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // Public Extern glTextureView(texture as int, iTarget As int, GLuint origtexture,internalformat as int, GLuint minlevel, GLuint numlevels, GLuint minlayer, GLuint numlayers)
 // Public Extern glBindVertexBuffer(GLuint bindingindex, buffer as int, offset as int, stride as int)
 // Public Extern glVertexAttribFormat(GLuint attribindex, size as int, iType As int, as byte normalized, GLuint relativeoffset)
 // Public Extern glVertexAttribIFormat(GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // Public Extern glVertexAttribLFormat(GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // Public Extern glVertexAttribBinding(GLuint attribindex, GLuint bindingindex)
 // Public Extern glVertexBindingDivisor(GLuint bindingindex, GLuint divisor)
 // Public Extern glConsole.WriteLine(MessageControl(as int source, iType As int, as int severity, iSize as int, ids as int[], as byte enabled)
 // Public Extern glConsole.WriteLine(MessageInsert(as int source, iType As int, id as int, as int severity, GLsizei length, const GLchar * buf)
 // Public Extern glConsole.WriteLine(MessageCallback(GLConsole.WriteLine(PROC callback, const * userParam)
 // GLAPI GLuint APIENTRY glGetConsole.WriteLine(MessageLog(iCount As int, BufSize As int, as int * sources, as int * types, GLuint * ids, as int * severities, legth as int[]s, GLchar * messageLog)
 // Public Extern glPushConsole.WriteLine(Group(as int source, id as int, GLsizei length, const GLchar * message)
 // Public Extern glPopConsole.WriteLine(Group()
 // Public Extern glObjectLabel(as int identifier, GLuint name, GLsizei length, const GLchar * label)
 // Public Extern glGetObjectLabel(as int identifier, GLuint name, BufSize As int, legth as int[], GLchar * label)
 // Public Extern glObjectPtrLabel(const * ptr, GLsizei length, const GLchar * label)
 // Public Extern glGetObjectPtrLabel(const * ptr, BufSize As int, legth as int[], GLchar * label)

 // Public Extern glBufferStorage(iTarget As int, size as int, data as byte[], GLbitfield flags)
 // Public Extern glClearTexImage(texture as int, iLevel as int, iFormat as int, iType As int, data as byte[])
 // Public Extern glClearTexSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, data as byte[])
 // Public Extern glBindBuffersBase(iTarget As int, GLuint first, iSize as int, const buffers as int[])
 // Public Extern glBindBuffersRange(iTarget As int, GLuint first, iSize as int, const buffers as int[], const GLintptr * offsets, const GLsizeiptr * sizes)
 // Public Extern glBindTextures(GLuint first, iSize as int, const GLuint * textures)
 // Public Extern glBindSamplers(GLuint first, iSize as int, const GLuint * samplers)
 // Public Extern glBindImageTextures(GLuint first, iSize as int, const GLuint * textures)
 // Public Extern glBindVertexBuffers(GLuint first, iSize as int, const buffers as int[], const GLintptr * offsets, const GLsizei * strides)

 // Public Extern glClipControl(as int origin, as int depth)
 // Public Extern glCreateTransformFeedbacks(n as int, GLuint * ids)
 // Public Extern glTransformFeedbackBufferBase(GLuint xfb, iIndex As int, buffer as int)
 // Public Extern glTransformFeedbackBufferRange(GLuint xfb, iIndex As int, buffer as int, offset as int, size as int)
 // Public Extern glGetTransformFeedbackiv(GLuint xfb, iPname As int, GLint * param)
 // Public Extern glGetTransformFeedbacki_v(GLuint xfb, iPname As int, iIndex As int, GLint * param)
 // Public Extern glGetTransformFeedbacki64_v(GLuint xfb, iPname As int, iIndex As int, GLint64 * param)
 // Public Extern glCreateBuffers(n as int, buffers as int[])
 // Public Extern glNamedBufferStorage(buffer as int, size as int, data as byte[], GLbitfield flags)
 // Public Extern glNamedBufferData(buffer as int, size as int, data as byte[], as int usage)
 // Public Extern glNamedBufferSubData(buffer as int, offset as int, size as int, data as byte[])
 // Public Extern glCopyNamedBufferSubData(GLuint readBuffer, GLuint writeBuffer, GLintptr readOffset, GLintptr writeOffset, size as int)
 // Public Extern glClearNamedBufferData(buffer as int,internalformat as int, iFormat as int, iType As int, data as byte[])
 // Public Extern glClearNamedBufferSubData(buffer as int,internalformat as int, offset as int, size as int, iFormat as int, iType As int, data as byte[])
 // GLAPI * APIENTRY glMapNamedBuffer(buffer as int, as int access)
 // GLAPI * APIENTRY glMapNamedBufferRange(buffer as int, offset as int, GLsizeiptr length, GLbitfield access)
 // GLAPI as byte APIENTRY glUnmapNamedBuffer(buffer as int)
 // Public Extern glFlushMappedNamedBufferRange(buffer as int, offset as int, GLsizeiptr length)
 // Public Extern glGetNamedBufferParameteriv(buffer as int, iPname As int, params as int[])
 // Public Extern glGetNamedBufferParameteri64v(buffer as int, iPname As int, GLint64 * params)
 // Public Extern glGetNamedBufferPointerv(buffer as int, iPname As int, * * params)
 // Public Extern glGetNamedBufferSubData(buffer as int, offset as int, size as int, data as byte[])
 // Public Extern glCreateFramebuffers(n as int, GLuint * framebuffers)
 // Public Extern glNamedFramebufferRenderbuffer(GLuint framebuffer, as int attachment, as int renderbuffertarget, GLuint renderbuffer)
 // Public Extern glNamedFramebufferParameteri(GLuint framebuffer, iPname As int, iParam As int)
 // Public Extern glNamedFramebufferTexture(GLuint framebuffer, as int attachment, texture as int, iLevel as int)
 // Public Extern glNamedFramebufferTextureLayer(GLuint framebuffer, as int attachment, texture as int, iLevel as int, GLint layer)
 // Public Extern glNamedFramebufferDrawBuffer(GLuint framebuffer, iBuff as int)
 // Public Extern glNamedFramebufferDrawBuffers(GLuint framebuffer, n as int, const as int * bufs)
 // Public Extern glNamedFramebufferReadBuffer(GLuint framebuffer, as int src)
 // Public Extern glInvalidateNamedFramebufferData(GLuint framebuffer, n as intumAttachments, const as int * attachments)
 // Public Extern glInvalidateNamedFramebufferSubData(GLuint framebuffer, n as intumAttachments, const as int * attachments, x As int, y As int, iWidth As int, iHeight As int)
 // Public Extern glClearNamedFramebufferiv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, value as int[])
 // Public Extern glClearNamedFramebufferuiv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, const GLuint * value)
 // Public Extern glClearNamedFramebufferfv(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, const GLsingle * value)
 // Public Extern glClearNamedFramebufferfi(GLuint framebuffer, iBuff as intfer, GLint drawbuffer, GLsingle depth, GLint stencil)
 // Public Extern glBlitNamedFramebuffer(GLuint readFramebuffer, GLuint drawFramebuffer, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GiMaskBitField as int, as int filter)
 // GLAPI as int APIENTRY glCheckNamedFramebufferStatus(GLuint framebuffer, iTarget As int)
 // Public Extern glGetNamedFramebufferParameteriv(GLuint framebuffer, iPname As int, GLint * param)
 // Public Extern glGetNamedFramebufferAttachmentParameteriv(GLuint framebuffer, as int attachment, iPname As int, params as int[])
 // Public Extern glCreateRenderbuffers(n as int, GLuint * renderbuffers)
 // Public Extern glNamedRenderbufferStorage(GLuint renderbuffer,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glNamedRenderbufferStorageMultisample(GLuint renderbuffer, GLsizei samples,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glGetNamedRenderbufferParameteriv(GLuint renderbuffer, iPname As int, params as int[])
 // Public Extern glCreateTextures(iTarget As int, n as int, GLuint * textures)
 // Public Extern glTextureBuffer(texture as int,internalformat as int, buffer as int)
 // Public Extern glTextureBufferRange(texture as int,internalformat as int, buffer as int, offset as int, size as int)
 // Public Extern glTextureStorage1D(texture as int, GLsizei levels,internalformat as int, iWidth As int)
 // Public Extern glTextureStorage2D(texture as int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int)
 // Public Extern glTextureStorage3D(texture as int, GLsizei levels,internalformat as int, iWidth As int, iHeight As int, iDepth as int)
 // Public Extern glTextureStorage2DMultisample(texture as int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, as byte fixedsamplelocations)
 // Public Extern glTextureStorage3DMultisample(texture as int, GLsizei samples,internalformat as int, iWidth As int, iHeight As int, iDepth as int, as byte fixedsamplelocations)
 // Public Extern glTextureSubImage1D(texture as int, iLevel as int, xOffset as int, iWidth As int, iFormat as int, iType As int, pixels as byte[])
 // Public Extern glTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, iWidth As int, iHeight As int, iFormat as int, iType As int, pixels as byte[])
 // Public Extern glTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, pixels as byte[])
 // Public Extern glCompressedTextureSubImage1D(texture as int, iLevel as int, xOffset as int, iWidth As int, iFormat as int, imagesize as int, data as byte[])
 // Public Extern glCompressedTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, iWidth As int, iHeight As int, iFormat as int, imagesize as int, data as byte[])
 // Public Extern glCompressedTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, imagesize as int, data as byte[])
 // Public Extern glCopyTextureSubImage1D(texture as int, iLevel as int, xOffset as int, x As int, y As int, iWidth As int)
 // Public Extern glCopyTextureSubImage2D(texture as int, iLevel as int, xOffset as int, yOffset as int, x As int, y As int, iWidth As int, iHeight As int)
 // Public Extern glCopyTextureSubImage3D(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, x As int, y As int, iWidth As int, iHeight As int)
 // Public Extern glTextureParameterf(texture as int, iPname As int, fParam As single)
 // Public Extern glTextureParameterfv(texture as int, iPname As int, const GLsingle * param)
 // Public Extern glTextureParameteri(texture as int, iPname As int, iParam As int)
 // Public Extern glTextureParameterIiv(texture as int, iPname As int, inxParams as int[])
 // Public Extern glTextureParameterIuiv(texture as int, iPname As int, const params as int[])
 // Public Extern glTextureParameteriv(texture as int, iPname As int, const GLint * param)
 // Public Extern glGenerateTextureMipmap(texture as int)
 // Public Extern glBindTextureUnit(GLuint unit, texture as int)
 // Public Extern glGetTextureImage(texture as int, iLevel as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // Public Extern glGetCompressedTextureImage(texture as int, iLevel as int, BufSize As int, * pixels)
 // Public Extern glGetTextureLevelParameterfv(texture as int, iLevel as int, iPname As int, params as single[])
 // Public Extern glGetTextureLevelParameteriv(texture as int, iLevel as int, iPname As int, params as int[])
 // Public Extern glGetTextureParameterfv(texture as int, iPname As int, params as single[])
 // Public Extern glGetTextureParameterIiv(texture as int, iPname As int, params as int[])
 // Public Extern glGetTextureParameterIuiv(texture as int, iPname As int, params as int[])
 // Public Extern glGetTextureParameteriv(texture as int, iPname As int, params as int[])
 // Public Extern glCreateVertexArrays(n as int, GLuint * arrays)
 // Public Extern glDisableVertexArrayAttrib(GLuint vaobj, iIndex As int)
 // Public Extern glEnableVertexArrayAttrib(GLuint vaobj, iIndex As int)
 // Public Extern glVertexArrayElementBuffer(GLuint vaobj, buffer as int)
 // Public Extern glVertexArrayVertexBuffer(GLuint vaobj, GLuint bindingindex, buffer as int, offset as int, stride as int)
 // Public Extern glVertexArrayVertexBuffers(GLuint vaobj, GLuint first, iSize as int, const buffers as int[], const GLintptr * offsets, const GLsizei * strides)
 // Public Extern glVertexArrayAttribBinding(GLuint vaobj, GLuint attribindex, GLuint bindingindex)
 // Public Extern glVertexArrayAttribFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, as byte normalized, GLuint relativeoffset)
 // Public Extern glVertexArrayAttribIFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // Public Extern glVertexArrayAttribLFormat(GLuint vaobj, GLuint attribindex, size as int, iType As int, GLuint relativeoffset)
 // Public Extern glVertexArrayBindingDivisor(GLuint vaobj, GLuint bindingindex, GLuint divisor)
 // Public Extern glGetVertexArrayiv(GLuint vaobj, iPname As int, GLint * param)
 // Public Extern glGetVertexArrayIndexediv(GLuint vaobj, iIndex As int, iPname As int, GLint * param)
 // Public Extern glGetVertexArrayIndexed64iv(GLuint vaobj, iIndex As int, iPname As int, GLint64 * param)
 // Public Extern glCreateSamplers(n as int, GLuint * samplers)
 // Public Extern glCreateProgramPipelines(n as int, GLuint * pipelines)
 // Public Extern glCreateQueries(iTarget As int, n as int, GLuint * ids)
 // Public Extern glGetQueryBufferObjecti64v(id as int, buffer as int, iPname As int, offset as int)
 // Public Extern glGetQueryBufferObjectiv(id as int, buffer as int, iPname As int, offset as int)
 // Public Extern glGetQueryBufferObjectui64v(id as int, buffer as int, iPname As int, offset as int)
 // Public Extern glGetQueryBufferObjectuiv(id as int, buffer as int, iPname As int, offset as int)
 // Public Extern glMemoryBarrierByRegion(GLbitfield barriers)
 // Public Extern glGetTextureSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // Public Extern glGetCompressedTextureSubImage(texture as int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, BufSize As int, * pixels)
 // GLAPI as int APIENTRY glGetGraphicsResetStatus()
 // Public Extern glGetnCompressedTexImage(iTarget As int, GLint lod, BufSize As int, * pixels)
 // Public Extern glGetnTexImage(iTarget As int, iLevel as int, iFormat as int, iType As int, BufSize As int, * pixels)
 // Public Extern glGetnUniformdv(program As int, location as int, BufSize As int, as float * params)
 // Public Extern glGetnUniformfv(program As int, location as int, BufSize As int, params as single[])
 // Public Extern glGetnUniformiv(program As int, location as int, BufSize As int, params as int[])
 // Public Extern glGetnUniformuiv(program As int, location as int, BufSize As int, params as int[])
 // Public Extern glReadnPixels(x As int, y As int, iWidth As int, iHeight As int, iFormat as int, iType As int, BufSize As int, data as byte[])
 // Public Extern glTextureBarrier()

 // Publicshader As intcializeShader(GLuint shader, const GLchar * pEntryPoint, GLuint numSpecializationConstants, const GLuint * pConstantIndex, const GLuint * pConstantValue)
 // Public Extern glMultiDrawArraysIndirectCount(Mode As int, const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // Public Extern glMultiDrawElementsIndirectCount(Mode As int, iType As int, const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // Public Extern glPolygonOffsetClamp(GLsingle factor, GLsingle units, GLsingle clamp)
 // Public Const GL_ARB_ES3_2_compatibility 1
 // Public Const GL_PRIMITIVE_BOUNDING_BOX_ARB As int = 0x92BE
 // Public Const GL_MULTISAMPLE_LINE_WIDTH_RANGE_ARB As int = 0x9381
 // Public Const GL_MULTISAMPLE_LINE_WIDTH_GRANULARITY_ARB As int = 0x9382
 // Public Extern glPrimitiveBoundingBoxARB(GLsingle minX, GLsingle minY, GLsingle minZ, GLsingle minW, GLsingle maxX, GLsingle maxY, GLsingle maxZ, GLsingle maxW)
 // GLAPI GLuint64 APIENTRY glGetTextureHandleARB(texture as int)
 // GLAPI GLuint64 APIENTRY glGetTextureSamplerHandleARB(texture as int, GLuint sampler)
 // Public Extern glMakeTextureHandleResidentARB(GLuint64 handle)
 // Public Extern glMakeTextureHandleNonResidentARB(GLuint64 handle)
 // GLAPI GLuint64 APIENTRY glGetImageHandleARB(texture as int, iLevel as int, as byte layered, GLint layer, iFormat as int)
 // Public Extern glMakeImageHandleResidentARB(GLuint64 handle, as int access)
 // Public Extern glMakeImageHandleNonResidentARB(GLuint64 handle)
 // Public Extern glUniformHandleui64ARB(location as int, GLuint64 value)
 // Public Extern glUniformHandleui64vARB(location as int, iSize as int, const GLuint64 * value)
 // Public Extern glProgramUniformHandleui64ARB(program As int, location as int, GLuint64 value)
 // Public Extern glProgramUniformHandleui64vARB(program As int, location as int, iSize as int, const GLuint64 * values)
 // GLAPI as byte APIENTRY glIsTextureHandleResidentARB(GLuint64 handle)
 // GLAPI as byte APIENTRY glIsImageHandleResidentARB(GLuint64 handle)
 // Public Extern glVertexAttribL1ui64ARB(iIndex As int, GLuint64EXT x)
 // Public Extern glVertexAttribL1ui64vARB(iIndex As int, const GLuint64EXT * v)
 // Public Extern glGetVertexAttribLui64vARB(iIndex As int, iPname As int, GLuint64EXT * params)
 // Public Const GL_ARB_cl_event 1
 // Struct _cl_context
 // Struct _cl_event
 // Public Const GL_SYNC_CL_EVENT_ARB As int = 0x8240
 // Public Const GL_SYNC_CL_EVENT_COMPLETE_ARB As int = 0x8241
 //
 // GLAPI GLsync APIENTRY glCreateSyncFromCLeventARB(struct _cl_context * context, struct _cl_event * Event , GLbitfield flags)

 // Public Extern glDispatchComputeGroupSizeARB(GLuint num_groups_x, GLuint num_groups_y, GLuint num_groups_z, GLuint group_size_x, GLuint group_size_y, GLuint group_size_z)
 //
 // Public Const GL_ARB_Console.WriteLine(_output 1
 // typedef(APIENTRY * GLConsole.WriteLine(PROCARB)(as int source, iType As int, id as int, as int severity, GLsizei length, const GLchar * message, const * userParam)

 // Public Extern glConsole.WriteLine(MessageControlARB(as int source, iType As int, as int severity, iSize as int, ids as int[], as byte enabled)
 // Public Extern glConsole.WriteLine(MessageInsertARB(as int source, iType As int, id as int, as int severity, GLsizei length, const GLchar * buf)
 // Public Extern glConsole.WriteLine(MessageCallbackARB(GLConsole.WriteLine(PROCARB callback, const * userParam)
 // GLAPI GLuint APIENTRY glGetConsole.WriteLine(MessageLogARB(iCount As int, BufSize As int, as int * sources, as int * types, GLuint * ids, as int * severities, legth as int[]s, GLchar * messageLog)
 //
 // Public Const GL_ARB_draw_buffers_blend 1
 //
 // Public Extern glBlendEquationiARB(GLuint buf, Mode As int)
 // Public Extern glBlendEquationSeparateiARB(GLuint buf, Mode As intRGB, Mode As intAlpha)
 // Public Extern glBlendFunciARB(GLuint buf, as int src, as int dst)
 // Public Extern glBlendFuncSeparateiARB(GLuint buf, as int srcRGB, as int dstRGB, as int srcAlpha, as int dstAlpha)
 //
 // Public Const GL_ARB_draw_instanced 1
 //
 // Public Extern glDrawArraysInstancedARB(Mode As int, first as int, iSize as int, GLsizei primcount)
 // Public Extern glDrawElementsInstancedARB(Mode As int, iSize as int, iType As int, const * indices, GLsizei primcount)
 //

 // Public Extern glProgramParameteriARB(program As int, iPname As int, GLint value)
 // Public Extern glFramebufferTextureARB(iTarget As int, as int attachment, texture as int, iLevel as int)
 // Public Extern glFramebufferTextureLayerARB(iTarget As int, as int attachment, texture as int, iLevel as int, GLint layer)
 // Public Extern glFramebufferTextureFaceARB(iTarget As int, as int attachment, texture as int, iLevel as int, face As int)
 //
 // Public Const GL_ARB_gl_spirv 1
 // Public Const GL_SHADER_BINARY_FORMAT_SPIR_V_ARB As int = 0x9551
 // Public Const GL_SPIR_V_BINARY_ARB As int = 0x9552
 //
 // Public Extern glSpecializeShadshader As intshader, const GLchar * pEntryPoint, GLuint numSpecializationConstants, const GLuint * pConstantIndex, const GLuint * pConstantValue)
 //

 // Public Extern glUniform1i64ARB(location as int, GLint64 x)
 // Public Extern glUniform2i64ARB(location as int, GLint64 x, GLint64 y)
 // Public Extern glUniform3i64ARB(location as int, GLint64 x, GLint64 y, GLint64 z)
 // Public Extern glUniform4i64ARB(location as int, GLint64 x, GLint64 y, GLint64 z, GLint64 w)
 // Public Extern glUniform1i64vARB(location as int, iSize as int, const GLint64 * value)
 // Public Extern glUniform2i64vARB(location as int, iSize as int, const GLint64 * value)
 // Public Extern glUniform3i64vARB(location as int, iSize as int, const GLint64 * value)
 // Public Extern glUniform4i64vARB(location as int, iSize as int, const GLint64 * value)
 // Public Extern glUniform1ui64ARB(location as int, GLuint64 x)
 // Public Extern glUniform2ui64ARB(location as int, GLuint64 x, GLuint64 y)
 // Public Extern glUniform3ui64ARB(location as int, GLuint64 x, GLuint64 y, GLuint64 z)
 // Public Extern glUniform4ui64ARB(location as int, GLuint64 x, GLuint64 y, GLuint64 z, GLuint64 w)
 // Public Extern glUniform1ui64vARB(location as int, iSize as int, const GLuint64 * value)
 // Public Extern glUniform2ui64vARB(location as int, iSize as int, const GLuint64 * value)
 // Public Extern glUniform3ui64vARB(location as int, iSize as int, const GLuint64 * value)
 // Public Extern glUniform4ui64vARB(location as int, iSize as int, const GLuint64 * value)
 // Public Extern glGetUniformi64vARB(program As int, location as int, GLint64 * params)
 // Public Extern glGetUniformui64vARB(program As int, location as int, GLuint64 * params)
 // Public Extern glGetnUniformi64vARB(program As int, location as int, BufSize As int, GLint64 * params)
 // Public Extern glGetnUniformui64vARB(program As int, location as int, BufSize As int, GLuint64 * params)
 // Public Extern glProgramUniform1i64ARB(program As int, location as int, GLint64 x)
 // Public Extern glProgramUniform2i64ARB(program As int, location as int, GLint64 x, GLint64 y)
 // Public Extern glProgramUniform3i64ARB(program As int, location as int, GLint64 x, GLint64 y, GLint64 z)
 // Public Extern glProgramUniform4i64ARB(program As int, location as int, GLint64 x, GLint64 y, GLint64 z, GLint64 w)
 // Public Extern glProgramUniform1i64vARB(program As int, location as int, iSize as int, const GLint64 * value)
 // Public Extern glProgramUniform2i64vARB(program As int, location as int, iSize as int, const GLint64 * value)
 // Public Extern glProgramUniform3i64vARB(program As int, location as int, iSize as int, const GLint64 * value)
 // Public Extern glProgramUniform4i64vARB(program As int, location as int, iSize as int, const GLint64 * value)
 // Public Extern glProgramUniform1ui64ARB(program As int, location as int, GLuint64 x)
 // Public Extern glProgramUniform2ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y)
 // Public Extern glProgramUniform3ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y, GLuint64 z)
 // Public Extern glProgramUniform4ui64ARB(program As int, location as int, GLuint64 x, GLuint64 y, GLuint64 z, GLuint64 w)
 // Public Extern glProgramUniform1ui64vARB(program As int, location as int, iSize as int, const GLuint64 * value)
 // Public Extern glProgramUniform2ui64vARB(program As int, location as int, iSize as int, const GLuint64 * value)
 // Public Extern glProgramUniform3ui64vARB(program As int, location as int, iSize as int, const GLuint64 * value)
 // Public Extern glProgramUniform4ui64vARB(program As int, location as int, iSize as int, const GLuint64 * value)
 //
 // Public Const GL_ARB_indirect_parameters 1
 // Public Const GL_PARAMETER_BUFFER_ARB As int = 0x80EE
 // Public Const GL_PARAMETER_BUFFER_BINDING_ARB As int = 0x80EF
 //
 // Public Extern glMultiDrawArraysIndirectCountARB(Mode As int, const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 // Public Extern glMultiDrawElementsIndirectCountARB(Mode As int, iType As int, const * indirect, GLintptr drawcount, GLsizei maxdrawcount, stride as int)
 //
 // Public Const GL_ARB_instanced_arrays 1
 // Public Const GL_VERTEX_ATTRIB_ARRAY_DIVISOR_ARB As int = 0x88FE
 //
 // Public Extern glVertexAttribDivisorARB(iIndex As int, GLuint divisor)
 //
 // Public Const GL_ARB_internalformat_query2 1
 // Public Const GL_SRGB_DECODE_ARB As int = 0x8299
 // Public Const GL_VIEW_CLASS_EAC_R11 As int = 0x9383
 // Public Const GL_VIEW_CLASS_EAC_RG11 As int = 0x9384
 // Public Const GL_VIEW_CLASS_ETC2_RGB As int = 0x9385
 // Public Const GL_VIEW_CLASS_ETC2_RGBA As int = 0x9386
 // Public Const GL_VIEW_CLASS_ETC2_EAC_RGBA As int = 0x9387
 // Public Const GL_VIEW_CLASS_ASTC_4x4_RGBA As int = 0x9388
 // Public Const GL_VIEW_CLASS_ASTC_5x4_RGBA As int = 0x9389
 // Public Const GL_VIEW_CLASS_ASTC_5x5_RGBA As int = 0x938A
 // Public Const GL_VIEW_CLASS_ASTC_6x5_RGBA As int = 0x938B
 // Public Const GL_VIEW_CLASS_ASTC_6x6_RGBA As int = 0x938C
 // Public Const GL_VIEW_CLASS_ASTC_8x5_RGBA As int = 0x938D
 // Public Const GL_VIEW_CLASS_ASTC_8x6_RGBA As int = 0x938E
 // Public Const GL_VIEW_CLASS_ASTC_8x8_RGBA As int = 0x938F
 // Public Const GL_VIEW_CLASS_ASTC_1 As int = & H5_RGBA As int = 0x9390
 // Public Const GL_VIEW_CLASS_ASTC_1 As int = & H6_RGBA As int = 0x9391
 // Public Const GL_VIEW_CLASS_ASTC_1 As int = & H8_RGBA As int = 0x9392
 // Public Const GL_VIEW_CLASS_ASTC_1 As int = & H10_RGBA As int = 0x9393
 // Public Const GL_VIEW_CLASS_ASTC_12x10_RGBA As int = 0x9394
 // Public Const GL_VIEW_CLASS_ASTC_12x12_RGBA As int = 0x9395
 //
 // Public Const GL_ARB_parallel_shader_compile 1
 // Public Const GL_MAX_SHADER_COMPILER_THREADS_ARB As int = 0x91B0
 // Public Const GL_COMPLETION_STATUS_ARB As int = 0x91B1
 // typedef(APIENTRYP PFNGLMAXSHADERCOMPILERTHREADSARBPROC)(iCount As int)
 //
 // Public Extern glMaxShaderCompilerThreadsARB(iCount As int)
 //
 // Public Const GL_ARB_pipeline_statistics_query 1
 // Public Const GL_VERTICES_SUBMITTED_ARB As int = 0x82EE
 // Public Const GL_PRIMITIVES_SUBMITTED_ARB As int = 0x82EF
 // Public Const GL_VERTEX_SHADER_INVOCATIONS_ARB As int = 0x82F0
 // Public Const GL_TESS_CONTROL_SHADER_PATCHES_ARB As int = 0x82F1
 // Public Const GL_TESS_EVALUATION_SHADER_INVOCATIONS_ARB As int = 0x82F2
 // Public Const GL_GEOMETRY_SHADER_PRIMITIVES_EMITTED_ARB As int = 0x82F3
 // Public Const GL_FRAGMENT_SHADER_INVOCATIONS_ARB As int = 0x82F4
 // Public Const GL_COMPUTE_SHADER_INVOCATIONS_ARB As int = 0x82F5
 // Public Const GL_CLIPPING_INPUT_PRIMITIVES_ARB As int = 0x82F6
 // Public Const GL_CLIPPING_OUTPUT_PRIMITIVES_ARB As int = 0x82F7
 //
 // Public Const GL_ARB_pixel_buffer_object 1
 // Public Const GL_PIXEL_PACK_BUFFER_ARB As int = 0x88EB
 // Public Const GL_PIXEL_UNPACK_BUFFER_ARB As int = 0x88EC
 // Public Const GL_PIXEL_PACK_BUFFER_BINDING_ARB As int = 0x88ED
 // Public Const GL_PIXEL_UNPACK_BUFFER_BINDING_ARB As int = 0x88EF
 //
 // Public Const GL_ARB_robustness 1
 // Public Const GL_CONTEXT_FLAG_ROBUST_ACCESS_BIT_ARB As int = 0x00000004
 // Public Const GL_LOSE_CONTEXT_ON_RESET_ARB As int = 0x8252
 // Public Const GL_GUILTY_CONTEXT_RESET_ARB As int = 0x8253
 // Public Const GL_INNOCENT_CONTEXT_RESET_ARB As int = 0x8254
 // Public Const GL_UNKNOWN_CONTEXT_RESET_ARB As int = 0x8255
 // Public Const GL_RESET_NOTIFICATION_STRATEGY_ARB As int = 0x8256
 // Public Const GL_NO_RESET_NOTIFICATION_ARB As int = 0x8261
 //
 // GLAPI as int APIENTRY glGetGraphicsResetStatusARB()
 // Public Extern glGetnTexImageARB(iTarget As int, iLevel as int, iFormat as int, iType As int, BufSize As int, * img)
 // Public Extern glReadnPixelsARB(x As int, y As int, iWidth As int, iHeight As int, iFormat as int, iType As int, BufSize As int, data as byte[])
 // Public Extern glGetnCompressedTexImageARB(iTarget As int, GLint lod, BufSize As int, * img)
 // Public Extern glGetnUniformfvARB(program As int, location as int, BufSize As int, params as single[])
 // Public Extern glGetnUniformivARB(program As int, location as int, BufSize As int, params as int[])
 // Public Extern glGetnUniformuivARB(program As int, location as int, BufSize As int, params as int[])
 // Public Extern glGetnUniformdvARB(program As int, location as int, BufSize As int, as float * params)
 //
 // Public Const GL_ARB_sample_locations 1
 // Public Const GL_SAMPLE_LOCATION_SUBPIXEL_BITS_ARB As int = 0x933D
 // Public Const GL_SAMPLE_LOCATION_PIXEL_GRID_WIDTH_ARB As int = 0x933E
 // Public Const GL_SAMPLE_LOCATION_PIXEL_GRID_HEIGHT_ARB As int = 0x933F
 // Public Const GL_PROGRAMMABLE_SAMPLE_LOCATION_TABLE_SIZE_ARB As int = 0x9340
 // Public Const GL_SAMPLE_LOCATION_ARB As int = 0x8E50
 // Public Const GL_PROGRAMMABLE_SAMPLE_LOCATION_ARB As int = 0x9341
 // Public Const GL_FRAMEBUFFER_PROGRAMMABLE_SAMPLE_LOCATIONS_ARB As int = 0x9342
 // Public Const GL_FRAMEBUFFER_SAMPLE_LOCATION_PIXEL_GRID_ARB As int = 0x9343
 //
 // Public Extern glFramebufferSampleLocationsfvARB(iTarget As int, start as int, iSize as int, const GLsingle * v)
 // Public Extern glNamedFramebufferSampleLocationsfvARB(GLuint framebuffer, start as int, iSize as int, const GLsingle * v)
 // Public Extern glEvaluateDepthValuesARB()
 //
 // Public Const GL_ARB_sample_shading 1
 // Public Const GL_SAMPLE_SHADING_ARB As int = 0x8C36
 // Public Const GL_MIN_SAMPLE_SHADING_VALUE_ARB As int = 0x8C37
 //
 // Public Extern glMinSampleShadingARB(value as single)
 //
 // Public Const GL_ARB_shading_language_include 1
 // Public Const GL_SHADER_INCLUDE_ARB As int = 0x8DAE
 // Public Const GL_NAMED_STRING_LENGTH_ARB As int = 0x8DE9
 // Public Const GL_NAMED_STRING_TYPE_ARB As int = 0x8DEA
 //
 // Public Extern glNamedStringARB(iType As int, GLint namelen, const name as string, GLint stringlen, const GLchar * string)
 // Public Extern glDeleteNamedStringARB(GLint namelen, const name as string)
 // Public Extern glCompileShaderIncludeARB(GLuint shader, iSize as int, const GLchar * const * path, const GLinshader As int
 // GLAPI as byte APIENTRY glIsNamedStringARB(GLint namelen, const name as string)
 // Public Extern glGetNamedStringARB(GLint namelen, const name as string, BufSize As int, GLint * stringlen, GLchar * string)
 // Public Extern glGetNamedStringivARB(GLint namelen, const name as string, iPname As int, params as int[])
 //
 // Public Const GL_ARB_shading_language_packing 1
 //
 // Public Const GL_ARB_sparse_buffer 1
 // Public Const GL_SPARSE_STORAGE_BIT_ARB As int = 0x0400
 // Public Const GL_SPARSE_BUFFER_PAGE_SIZE_ARB As int = 0x82F8
 //
 // Public Extern glBufferPageCommitmentARB(iTarget As int, offset as int, size as int, as byte commit)
 // Public Extern glNamedBufferPageCommitmentEXT(buffer as int, offset as int, size as int, as byte commit)
 // Public Extern glNamedBufferPageCommitmentARB(buffer as int, offset as int, size as int, as byte commit)
 //
 // Public Const GL_ARB_sparse_texture 1
 // Public Const GL_TEXTURE_SPARSE_ARB As int = 0x91A6
 // Public Const GL_VIRTUAL_PAGE_SIZE_INDEX_ARB As int = 0x91A7
 // Public Const GL_NUM_SPARSE_LEVELS_ARB As int = 0x91AA
 // Public Const GL_NUM_VIRTUAL_PAGE_SIZES_ARB As int = 0x91A8
 // Public Const GL_VIRTUAL_PAGE_SIZE_X_ARB As int = 0x9195
 // Public Const GL_VIRTUAL_PAGE_SIZE_Y_ARB As int = 0x9196
 // Public Const GL_VIRTUAL_PAGE_SIZE_Z_ARB As int = 0x9197
 // Public Const GL_MAX_SPARSE_TEXTURE_SIZE_ARB As int = 0x9198
 // Public Const GL_MAX_SPARSE_3D_TEXTURE_SIZE_ARB As int = 0x9199
 // Public Const GL_MAX_SPARSE_ARRAY_TEXTURE_LAYERS_ARB As int = 0x919A
 // Public Const GL_SPARSE_TEXTURE_FULL_ARRAY_CUBE_MIPMAPS_ARB As int = 0x91A9
 //
 // Public Extern glTexPageCommitmentARB(iTarget As int, iLevel as int, xOffset as int, yOffset as int, zOffset as int, iWidth As int, iHeight As int, iDepth as int, as byte commit)
 // Public Const GL_ARB_sparse_texture2 1
 // Public Const GL_ARB_sparse_texture_clamp 1
 // Public Const GL_ARB_spirv_extensions 1
 // Public Const GL_ARB_stencil_texturing 1
 // Public Const GL_ARB_sync 1
 // Public Const GL_ARB_tessellation_shader 1
 // Public Const GL_ARB_texture_barrier 1
 // Public Const GL_ARB_texture_border_clamp 1
 // Public Const GL_CLAMP_TO_BORDER_ARB As int = 0x812D
 // Public Const GL_ARB_texture_buffer_object 1
 // Public Const GL_TEXTURE_BUFFER_ARB As int = 0x8C2A
 // Public Const GL_MAX_TEXTURE_BUFFER_SIZE_ARB As int = 0x8C2B
 // Public Const GL_TEXTURE_BINDING_BUFFER_ARB As int = 0x8C2C
 // Public Const GL_TEXTURE_BUFFER_DATA_STORE_BINDING_ARB As int = 0x8C2D
 // Public Const GL_TEXTURE_BUFFER_FORMAT_ARB As int = 0x8C2E
 // typedef(APIENTRYP PFNGLTEXBUFFERARBPROC)(iTarget As int,internalformat as int, buffer as int)
 // Public Extern glTexBufferARB(iTarget As int,internalformat as int, buffer as int)
 // Public Const GL_ARB_texture_buffer_object_rgb32 1
 // Public Const GL_ARB_texture_buffer_range 1
 // Public Const GL_ARB_texture_compression_bptc 1
 // Public Const GL_COMPRESSED_RGBA_BPTC_UNORM_ARB As int = 0x8E8C
 // Public Const GL_COMPRESSED_SRGB_ALPHA_BPTC_UNORM_ARB As int = 0x8E8D
 // Public Const GL_COMPRESSED_RGB_BPTC_SIGNED_single_ARB As int = 0x8E8E
 // Public Const GL_COMPRESSED_RGB_BPTC_UNSIGNED_single_ARB As int = 0x8E8F
 // Public Const GL_ARB_texture_compression_rgtc 1
 // Public Const GL_ARB_texture_cube_map_array 1
 // Public Const GL_TEXTURE_CUBE_MAP_ARRAY_ARB As int = 0x9009
 // Public Const GL_TEXTURE_BINDING_CUBE_MAP_ARRAY_ARB As int = 0x900A
 // Public Const GL_PROXY_TEXTURE_CUBE_MAP_ARRAY_ARB As int = 0x900B
 // Public Const GL_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900C
 // Public Const GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW_ARB As int = 0x900D
 // Public Const GL_INT_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900E
 // Public Const GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY_ARB As int = 0x900F
 // Public Const GL_ARB_texture_filter_anisotropic 1
 //
 // Public Const GL_ARB_texture_filter_minmax 1
 // Public Const GL_TEXTURE_REDUCTION_MODE_ARB As int = 0x9366
 // Public Const GL_WEIGHTED_AVERAGE_ARB As int = 0x9367
 //
 // Public Const GL_ARB_texture_gather 1
 // Public Const GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET_ARB As int = 0x8E5E
 // Public Const GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET_ARB As int = 0x8E5F
 // Public Const GL_MAX_PROGRAM_TEXTURE_GATHER_COMPONENTS_ARB As int = 0x8F9F
 //
 // Public Const GL_ARB_transform_feedback_overflow_query 1
 // Public Const GL_TRANSFORM_FEEDBACK_OVERFLOW_ARB As int = 0x82EC
 // Public Const GL_TRANSFORM_FEEDBACK_STREAM_OVERFLOW_ARB As int = 0x82ED

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
public static void VBOFlushAll()
    {


    VBO_vertex.Clear;
    VBO_colors.Clear;
    VBO_normals.Clear;
    VBO_pixels.Clear;

    VBO_vertex = new float[][];
    VBO_colors = new float[][];
    VBO_normals = new float[][];
    VBO_pixels = new float[][];

}

 // Clears the VBO from memory
public static void VBOFlush(int Id)
    {


    VBO_vertex[id].Clear;
    VBO_colors[id].Clear;
    VBO_normals[id].Clear;
    VBO_pixels[id].Clear;

}

public static void Resize(Object drwContext) // only glArea for now
    {


     // establecemos adonde vamos a dibujar, porque puede ser en algun lugar mas chico de la misma glDrawingArea
     // pero en general sera en todo el control
     // 2022 la siguiente linea parece estar deprecada, pongo cualquiera o simplemente la comento y funciona igual
     //gl.Viewport(100, 100, drwContext.W / 2, drwContext.h / 2)

    if ( ! Me.Initialized ) return; //Me.Init(drwContext)
     //    Console.WriteLine( "New GL Sheet Init:", drwContext.w, drwContext.H
     // le avisamos a GL que queremos usar texturas
    gl.Enable(gl.TEXTURE_2D);

     // borramos lo que haya dibujado
    gl.Clear(gl.DEPTH_BUFFER_BIT Or gl.COLOR_BUFFER_BIT);

     // le decimos que queremos usar cosas 3D y lo que esta mas lejos quede tapado por lo que esta mas cerca, esto en 2D no es necesario
    gl.Enable(gl.DEPTH_TEST);

     // no tengo idea, debe ser algo de AntiAlias
    gl.Enable(gl.SMOOTH);
    gl.Enable(gl.BLEND);
    gl.BlendFunc(GL.SRC_ALPHA, GL.ONE_MINUS_SRC_ALPHA);
     // usaremos luces
     // Gl.Enable(Gl.LIGHTING)
     // Gl.Enable(Gl.LIGHT0)
     // esto le dice que las normales las tiene que normalizar a 1
    Gl.Enable(Gl.NORMALIZE);

     // las coordenadas que le pasemos deberan estar entre -1 y 1
     // todo lo que sea > 1 NO SE MOSTRARA

    gl.MatrixMode(gl.PROJECTION);
    gl.LoadIdentity;
     //Glu.Ortho2D(-drwContext.w / 2, drwContext.w / 2, -drwContext.h / 2, drwContext.h / 2)
    Gl.Ortho(-drwContext.w / 2, drwContext.w / 2, -drwContext.h / 2, drwContext.h / 2, 1, -1);

    gl.MatrixMode(gl.MODELVIEW);
    gl.LoadIdentity;

}

public static void Init(Object drwContext) // only glArea for now
    {


     //Resize(drwContext)
    Console.WriteLine( "Init OpenGL");

    int gbcolor ;         
    double r ;         
    double g ;         
    double b ;         
    double a ;         

    gbcolor = drwContext.background;

    a = 1 - Color.GetAlpha(gbColor) / 255;
    r = (Shr(gbColor, 16) And 255) / 255;
    g = (Shr(gbColor, 8) And 255) / 255;
    b = (gbColor And 255) / 255;

    gl.ClearColor(r, g, b, a);

     // line stipples

    this.glFlush();
    Console.WriteLine( "libGL access: ok");
    Console.WriteLine( "Support VBO: " + gl.CheckExtensions("GL_ARB_vertex_buffer_object");
    Console.WriteLine( "Support GLSL: " + gl.CheckExtensions("GL_ARB_vertex_program");
    Console.WriteLine( "Shading version: " + GL.GetString(gl.SHADING_LANGUAGE_VERSION);
    Console.WriteLine( "GL version: " + GL.GetString(gl.VERSION);
    Console.WriteLine( "Chipset vendor: " + GL.GetString(gl.VENDOR);
     //gcd.Console.WriteLine(info("Extensions" &  GL.GetString(gl.EXTENSIONS))
    this.Initialized = true;

}
 // Dibuja un rectangulo, relleno o vacio.
 // Puede dibujar un contorno del mismo de otro color->Bounding
 // Mode: 0=relleno, 1=relleno y recuadro, 2=solo recuadro

public static void Rectangle2D(double x1, double y1, double w, double h, int colour1= Color.Blue, int colour2= -14, int colour3= -14, int colour4= -14, int BoundingColor= Color.Blue, int BoundingWIdth= 1, double[] Dashes= [], int mode= 0)
    {


    int c2 ;         
    int c3 ;         
    int c4 ;         
     Float[] flxVertex ;
        // Quad esta obsoleto , reemplazo por dos triangulos
        if (mode == 0 || mode == 1)
        {

            if (colour2 == -14)
            {
                c2 = colour1 
            } else {
                 c2 = colour2
                 };
        if ( colour3 == -14 ) c3 = colour1 } else { c3 = colour3;
        if ( colour4 == -14 ) c4 = colour1 } else { c4 = colour4;

        gl.begin(gl.TRIANGLES);

        Vertex2D(x1, y1, colour1);
        Vertex2D(x1 + w, y1, c2);
        Vertex2D(x1 + w, y1 + h, c3);

        Vertex2D(x1, y1, colour1);
        Vertex2D(x1 + w, y1 + h, c4);
        Vertex2D(x1, y1 + h, c3);

        gl.end;

    }

    if ( mode >= 1 ) // solo recuadro
    {

        PolyLines([x1, y1, x1 + w, y1, x1 + w, y1 + h, x1, y1 + h, x1, y1], BoundingColor, BoundingWIdth, dashes);

    }

}

 // Dibuja un rombo, relleno o vacio.
 // Puede dibujar un contorno del mismo de otro color->Bounding
 // Mode: 0=relleno, 1=relleno y recuadro, 2=solo recuadro
public static void Rombo2D(double x1, double y1, double side, int ColorLeft= Color.Blue, int ColorRigth= Color.Blue, int iDirection= 0, int BoundingColor= Color.Blue, int BoundingWIdth= 1, int mode= 0, double Rotation= 0)
    {


     Float[] flxVertex ;         
     // Quad esta obsoleto , reemplazo por dos triangulos
    side /= 2;
    if ( mode == 0 || mode == 1 )
    {

        gl.begin(gl.TRIANGLES);

        Vertex2D(x1 - side, y1, ColorLeft);
        Vertex2D(x1, y1 + side, ColorLeft);
        Vertex2D(x1, y1 - side, ColorLeft);

        Vertex2D(x1 + side, y1, ColorRigth);
        Vertex2D(x1, y1 + side, ColorRigth);
        Vertex2D(x1, y1 - side, ColorRigth);

        gl.end;

    }

    if ( mode >= 1 ) // solo recuadro
    {

        PolyLines([x1 - side, y1, x1, y1 + side, x1 + side, y1, x1, y1 - side, x1 - side, y1], BoundingColor);

    }

}

 // Dibuja un serie de lineas
 public static void DrawLines(float[] fVertices,  int colour = 0, double LineWIdth = 1, double[] dashes = null)
    {

    int i ;         
    double r ;         
    double g ;         
    double b ;         
    double[] Vertices ;         

     //If gbcolor > 0 Then Stop
    r = (Shr(Colour, 16) And 255) / 256;
    g = (Shr(Colour, 8) And 255) / 256;
    b = (Colour And 255) / 256;

    if ( fvertices.Count < 2 ) return;

    if ( InmediateMode )
    {

        gl.LineWIdth(LineWIdth); // obsoleto en WebGL
        if ( dashes.Count > 0 )
        {
            Vertices = puntos.DashedLineStrip(fvertices, dashes, 1);
        }

            Vertices = fvertices;

        }

        gl.Begin(gl.Lines);
        gl.Color3f(r, g, b);
        for ( i = 0; i <= vertices.Max; i + 2)
        {

             //glColorRGB(colour)
            gl.Vertex2f(vertices[i], vertices[i + 1]);

        }
        gl.end;

    } // we as writing to an array
        for ( i = 0; i <= vertices.Max; i + 2)
        {
            VBO_vertex[VBO_Id].Add(vertices[i]); //X
            VBO_vertex[VBO_Id].Add(vertices[i + 1]); //Y
            VBO_vertex[VBO_Id].Add(0); //Z

            VBO_colors[VBO_Id].Add(r); //R
            VBO_colors[VBO_Id].Add(g); //G
            VBO_colors[VBO_Id].Add(b); //B

            VBO_normals[VBO_Id].Add(0); //Normales apuntan al user
            VBO_normals[VBO_Id].Add(0); //
            VBO_normals[VBO_Id].Add(1); //

        }

    }

}

 // Dibuja un seria de polilinea
 public static void PolygonFilled(float[] vertices,  int colour = 0, int FillColor = 0, double LineWIdth = 1, double[] dashes = null)
    {

    int i ;         

     //glColorRGB(colour)
    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    if ( dashes )
    {
        gl.LineStipple(LineStippleScales[dashes], LineStipples[dashes]);
        gl.Enable(GL.LINE_STIPPLE);
    }

        gl.Disable(GL.LINE_STIPPLE);

    }

    gl.Begin(gl.POLYGON);

    for ( i = 0; i <= vertices.Max; i + 2)
    {
        glColorRGB(colour);
        gl.Vertex2f(vertices[i], vertices[i + 1]);

    }
    gl.end;

}

 // Dibuja un seria de polilinea
 public  Polygon(vertices As Float[],  colour As int = 0, LineWIdth As double = 1, dashes As double[]);

    int i ;         

     //glColorRGB(colour)
    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    gl.Begin(gl.LINE_LOOP);

    for ( i = 0; i <= vertices.Max; i + 2)
    {
        glColorRGB(colour);
        gl.Vertex2f(vertices[i], vertices[i + 1]);

    }
    gl.end;

}
 // Dibuja un arco, suponiendo que el centro esta en 0,0 (despues de un Translate())
 // Siempre gira en sentido anti-horario
 // Las medidas de los angulo inicial y recorrido estan en RADIANES

 public  ARC(radio As Float, start_angle As Float, length As Float,  tramos As int = 36, colour As int = 0, LineWIdth As double = 1, dashes As double[] = []);

    GL.Begin(gl.LINE_STRIP);
    double theta ;         
    double angle_increment ;         

     //double max_angle = 2 * Math.PI
     //double angle_increment = Math.PI / 1000
    if ( tramos <= 0 ) tramos = 36;
    if ( tramos > 360 ) tramos = 36;

    angle_increment = Math.PI * 2 / tramos;

     // verifico los angulos
     // If length < 0 Then angle_increment *= -1

    for ( theta = 0; theta <= length; theta + angle_increment)
    {

        Vertex2D(radio * Cos(start_angle + theta), radio * Sin(start_angle + theta), colour);
    }

    GL.End;

}

 public  ArcPoly(xCenter As Float, yCenter As Float, radio As Float, start_angle As Float, length As Float,  angle_increment As double = Math.PI * 2 / 360) As double[];

    double theta ;         
    double x0 ;         
    double y0 ;         
     Float[] flxPoly ;         
    int i ;         
     //double max_angle = 2 * Math.PI
     //double angle_increment = Math.PI / 1000
     //angle_increment = Pi * 2 / 360

     // verifico los angulos
     // If length < 0 Then angle_increment *= -1
    flxPoly.Resize(CInt(length / angle_increment) * 4);
    for ( theta = angle_increment; theta <= length; theta + angle_increment)
    {
        x0 = radio * Cos(start_angle + theta - angle_increment);
        y0 = radio * Sin(start_angle + theta - angle_increment);
        flxPoly[i] = x0 + xCenter;
        Inc i;
        flxPoly[i] = y0 + yCenter;
        Inc i;
        x0 = radio * Cos(start_angle + theta);
        y0 = radio * Sin(start_angle + theta);
        flxPoly[i] = x0 + xCenter;
        Inc i;
        flxPoly[i] = y0 + yCenter;
        Inc i;

    }
    x0 = radio * Cos(start_angle + theta - angle_increment);
    y0 = radio * Sin(start_angle + theta - angle_increment);

    if ( flxPoly.Count == i )
    {
        flxPoly.Add(0);
        flxPoly.Add(0);
    }
    flxPoly[i] = x0 + xCenter;
    Inc i;
    flxPoly[i] = y0 + yCenter;
    Inc i;

    x0 = radio * Cos(start_angle + length);
    y0 = radio * Sin(start_angle + length);

    if ( flxPoly.Count == i )
    {
        flxPoly.Add(0);
        flxPoly.Add(0);
    }
    flxPoly[i] = x0 + xCenter;
    Inc i;
    flxPoly[i] = y0 + yCenter;
    Inc i;
     //If flxPoly.Count <> i Then Stop

    return flxPoly;

}

 public  PolyLines(fVertices As Float[],  colour As int = 0, LineWIdth As double = 1, dashes As double[] = []);

    int i ;         
    double[] vertices ;         
    double[] vertices2 ;         
    double r ;         
    double g ;         
    double b ;         

    r = (Shr(Colour, 16) And 255) / 256;
    g = (Shr(Colour, 8) And 255) / 256;
    b = (Colour And 255) / 256;

    if ( fvertices.Count < 2 ) return;
    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    if ( dashes.count > 0 )
    {
        Vertices = puntos.DashedLineStrip(fvertices, dashes, 1);

        gl.Begin(gl.LINES);
        gl.Color3f(r, g, b);
        for ( i = 0; i <= vertices.Max; i + 2)
        {
             //    glColorRGB(colour)
            gl.Vertex2f(vertices[i], vertices[i + 1]);
        }

    }

        Vertices = fvertices;
        gl.Begin(gl.LINE_STRIP);
        gl.Color3f(r, g, b);
        for ( i = 0; i <= vertices.Max; i + 2)
        {
             //glColorRGB(colour)
            gl.Vertex2f(vertices[i], vertices[i + 1]);
        }

    }

    gl.end;

}

 public  DrawTriangles(vertices As Float[],  colour As int = 0, FillColor As int = 0, LineWIdth As double = 1, dashes As double[] = []);

    int i ;         

     //glColorRGB(colour)
    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    if ( dashes.Count > 0 )
    {

        PolyLines(vertices, colour, linewIdth, dashes);

    }

    gl.Begin(gl.TRIANGLES);

    for ( i = 0; i <= vertices.Max; i + 2)
    {
        glColorRGB(colour);
        gl.Vertex3f(vertices[i], vertices[i + 1], zLevel);

    }
    gl.end;

}

 public  DrawTriangles3D(vertices3D As Float[], faces As Float[],  colour As int = 0, FillColor As int = 0, LineWIdth As double = 1);

    int i ;         

     //glColorRGB(colour)
    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    gl.Begin(gl.TRIANGLES);

    for ( i = 0; i <= faces.max; i + 3)
    {

        glColorRGB(colour);
        gl.Vertex3f(vertices3d[faces[i]], vertices3d[faces[i + 1]], vertices3d[faces[i + 2]]);

    }
    gl.end;

}

 // Dibuja un circulo

public static void CIRCLE(double[] center, double radious, int colour= 0, bool Filled= false, double LineWIdth= 1, double[] dashes= [])
    {


    double x ;         
    double y ;         
    double theta ;         
    double angle_increment ;         
    int StepFactor = 2;
     Float[] vertices ;         
     Float[] fVertices ;         

    gl.LineWIdth(LineWIdth); // obsoleto en WebGL

    if ( filled )
    {
        GL.Begin(gl.POLYGON);
        angle_increment = Math.PI * 2 / 360; // esto va de a un grado, que puede ser exagerado

        angle_increment *= StepFactor;
        for ( theta = 0; theta <= 2 * Math.PI; theta + angle_increment)
        {
             // el punto considerando 0,0 al centro
            x = center[0] + radious * Math.Cos(theta);
            y = center[1] + radious * Math.Sin(theta);
            Vertex2D(x, y, Colour);
        }
        gl.End;
    }

        angle_increment = Math.PI * 2 / 360; // esto va de a un grado, que puede ser exagerado

        angle_increment *= StepFactor;

        for ( theta = 0; theta <= 2 * Math.PI; theta + angle_increment)
        {
             // el punto considerando 0,0 al centro
            x = center[0] + radious * Math.Cos(theta);
            y = center[1] + radious * Math.Sin(theta);
            fvertices.Add(x);
            fvertices.Add(y);
        }

        PolyLines(fVertices, colour, linewIdth, dashes);

    }

}

 public void glColorRGB(gbColor As int,  alpha As double = 1.0);
     // set the color to GL

    double r ;         
    double g ;         
    double b ;         
    double a ;         

    a = alpha; // 1 - Color.GetAlpha(gbColor) / 255
    r = (Shr(gbColor, 16) And 255) / 255;
    g = (Shr(gbColor, 8) And 255) / 255;
    b = (gbColor And 255) / 255;
    gl.Color4f(r, g, b, a);

}

 public  GetColorRGBA(gbColor As int) As float[];
     // set the color to GL

    double r ;         
    double g ;         
    double b ;         
    double a ;         

    a = 1 - Color.GetAlpha(gbColor) / 255;
    r = (Shr(gbColor, 16) And 255) / 255;
    g = (Shr(gbColor, 8) And 255) / 255;
    b = (gbColor And 255) / 255;

    return [r, g, b, a];

}

public static void ClearColor(int iColor)
    {


    float[] rgba ;         

    rgba = GetColorRGBA(iColor);
    gl.ClearColor(rgba[0], rgba[1], rgba[2], rgba[3]);

}
 // Define un ColorMaterial: rgba ambient , rgba diffuse, rgba specular, shininess (13 valores en total)

public static void glMaterial(double[] fMaterial)
    {


    gl.Materialfv(gl.FRONT_AND_BACK, gl.AMBIENT, fMaterial.Copy(0, 4));
    gl.Materialfv(gl.FRONT_AND_BACK, gl.DIFFUSE, fMaterial.Copy(4, 4));
    gl.Materialfv(gl.FRONT_AND_BACK, gl.SPECULAR, fMaterial.Copy(8, 4));
    gl.Materiali(gl.FRONT_AND_BACK, gl.SHININESS, CInt(fMaterial[12]));

}

public static void glMaterialHierro(double Alpha= 0)
    {

     // set the color to GL

    double r ;         
    double g ;         
    double b ;         
     Float[] MyColor ;         

    MyColor = [0.05375, 0.05, 0.06625, 1.0];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.AMBIENT, MyColor);

    MyColor = [0.18275, 0.17, 0.22525];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.DIFFUSE, MyColor);

    MyColor = [0.332741, 0.328634, 0.346435];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.SPECULAR, MyColor);

    gl.Materialf(gl.FRONT_AND_BACK, gl.SHININESS, 0.3 * 128);

}

public static void glMaterialMadera(double Alpha= 0)
    {

     // set the color to GL

    double r ;         
    double g ;         
    double b ;         
     Float[] MyColor ;         

    MyColor = [0.05, 0.05, 0.0, 1.0];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.AMBIENT, MyColor);

    MyColor = [0.5, 0.5, 0.4];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.DIFFUSE, MyColor);

    MyColor = [0.7, 0.7, 0.04];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.SPECULAR, MyColor);

    gl.Materialf(gl.FRONT_AND_BACK, gl.SHININESS, 0.07815 * 128);

}

public static void glMaterialConcreto(double Alpha= 0)
    {

     // set the color to GL

     Float[] MyColor ;         

    MyColor = [0.2, 0.2, 0.2, 1.0];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.AMBIENT, MyColor);

    MyColor = [0.6, 0.6, 0.6];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.DIFFUSE, MyColor);

    MyColor = [1, 1, 1];

    gl.Materialfv(gl.FRONT_AND_BACK, gl.SPECULAR, MyColor);

    gl.Materialf(gl.FRONT_AND_BACK, gl.SHININESS, 64);

}

 public void Vertex2D(x2d As Float, y2d As Float,  colour As int = Color.Red);
     //
     //
     //     //2020 el color va primero
     // //

    if ( colour )
    {
        this.glColorRGB(colour);

    }

    gl.Vertex2f(x2d, y2d);
     //

}

public void Vertex3D(p As Punto3d,  colour As int = Color.blue);
     //
     //
     //     //2020 el color va primero
     //

    glx.glColorRGB(colour);

    gl.Vertex3f(p.x, p.y, p.z);
     //

}
 //

public static void Normal3D(Punto3d p)
    {

     //
     //     gl.Normal3f(p.x, p.y, p.z)
     // //

}

public static void Get2DpointFrom3Dworld(Punto3d p1, double ByRef x2, double ByRef y2, double ByRef z2)
    {


     Float[16] modelmatrix ;         
     Float[16] projMatrix ;         
     Float[16] miMatrix ;         

     int[4] vp ;         

     Float[3] p2 ;         

    modelMatrix = gl.GetFloatv(GL.MODELVIEW_MATRIX);
    projMatrix = gl.GetFloatv(GL.PROJECTION_MATRIX);
    vp = gl.Getintv(GL.VIEWPORT_);

    p2 = glu.Project(p1.x, p1.y, p1.z, modelMatrix, projMatrix, vp);

    if ( IsNull(p2) ) return;

    x2 = p2[0];
    y2 = p2[1];

     // z>0 ---> el punto es visible
    z2 = p2[2];

}

public static void Get2DpointFrom3Dworld2(double x, double y, double z, double ByRef x2, double ByRef y2, double ByRef z2)
    {


     Float[16] modelmatrix ;         
     Float[16] projMatrix ;         
     Float[16] miMatrix ;         

     int[4] vp ;         

     Float[3] p2 ;         

    modelMatrix = gl.GetFloatv(GL.MODELVIEW_MATRIX);
    projMatrix = gl.GetFloatv(GL.PROJECTION_MATRIX);
    vp = gl.Getintv(GL.VIEWPORT_);

    p2 = glu.Project(x, y, z, modelMatrix, projMatrix, vp);

    if ( IsNull(p2) ) return;

    x2 = p2[0];
    y2 = p2[1];

     // z>0 ---> el punto es visible
    z2 = p2[2];

}

 // Proveo las matrices de trnasformacion porque en cada cambio deben guardarse para que esta funcion se corresponda con lo que se muestra
public static void Get2DpointFrom3Dworld3(double x, double y, double z, double[] ModelMatrix, double[] projMatrix, int[] iViewPort, double ByRef x2, double ByRef y2, double ByRef z2)
    {


     Float[3] p2 ;         

    p2 = glu.Project(x, y, z, modelMatrix, projMatrix, iViewPort);

    if ( IsNull(p2) ) return;

    x2 = p2[0];
    y2 = p2[1];

     // z>0 ---> el punto es visible
    z2 = p2[2];

}
 // Trnasforma un punto de la pantalla en un punto del espacio, que en realidad es un rayo en 3D
 // que en 2D es perpendicular a la pantalla

public static double[] Get3DpointFromScreen(int Xscreen, int Yscreen)
    {


     Float[16] modelmatrix ;         
     Float[16] projMatrix ;         
     Float[16] miMatrix ;         

     int[4] vp ;         

     Float[3] p2 ;         

    modelMatrix = gl.GetFloatv(GL.MODELVIEW_MATRIX);
    projMatrix = gl.GetFloatv(GL.PROJECTION_MATRIX);
    vp = gl.Getintv(GL.VIEWPORT_);

    p2 = glu.UnProject(Xscreen, Yscreen, 0, modelMatrix, projMatrix, vp);

    if ( IsNull(p2) ) return;

     // p2[2]
     // z<=1  --> el punto esta frente a la camara
     // z> 1  ---> el punto es invisible

     // test
     //Console.WriteLine( "Screen", Xscreen, Yscreen, " -> Real ", p2[0], p2[1]

    return p2;

}

 // Establece la fuente con que se dibujaran los textos
public static bool SelectFont(string FontName)
    {


    if ( glFont.Exist(FontName) )
    {
        ActualFont = glFont[FontName];
        return true; // fuente encontrada

    }

    return false; // fuente no encontrada

}

 // Lee todas las fuentes del directorio provisto y devuelve un listado con sus nombres
public static string[] LoadFonts(string DirPath)
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
     // #License: Public domain
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

    string sFilename ;         
    File fFile ;         
    string sData ;         
    string sCoord ;         
    string aVert ;         
    string sCode ;         
    string[] sPuntos ;         
     string[] Lista ;         
    int p1 ;         
    bool BulgeAdded ;         
    string[] sVert ;         
    float[] fltb ;         
    float[] flt1 ;         
    float[] flt3 ;         
    float flt2 ;         

    foreach ( sFilename in Dir(DirPath, "*.lff"))
    {

         LFFFonts fntNuevas ;         

        fntNuevas.FileName = sFilename;
        fntNuevas.WordSpacing = 6.75;
        fntNuevas.LetterSpacing = 1; //.25
        fntNuevas.LineSpacingFactor = 1;
        fntNuevas.Letter = new Dictionary<string, string>;

        ffile = Open DirPath &/ sFilename for ( Input;
        do {
            Line Input #fFile, sDAta;
            if ( Left$(sData, 1) == "[" ) // nueva letra
            {
                Dim Letra As new Letters;
                sCode = Replace(sData, "#", "");
                sCode = Replace(sCode, "[[", "[");
                sCode = Mid(sCode, 2, 4);
                letra.Code = Val("0x0000" + sCode); // [0021]!
                letra.FontGlyps = new float[][];
                letra.FontBulges = new float[][];

                While sdata <> "");
                    Line Input #fFile, sDAta; // 0.428572, 0.857143; 0, 0.428572
                    if ( Left(sData, 1) == "C" ) // Copio datos de otra letra
                    {
                        Dim CopyCode As int;
                        CopyCode = Val("0x0000" + Mid(sData, 2, 4)); // C0021
                         //CopyCode = GetCodeIndex(fntNuevas, CopyCode)
                        flt1 = new float[];
                        flt3 = new float[];

                        foreach ( flt1 in fntNuevas.Letter[CopyCode].FontGlyps)
                        {
                            letra.FontGlyps.Add(flt3);
                            foreach ( flt2 in flt1)
                            {
                                flt3.Add(flt2);
                            }
                        }
                        flt3 = new float[];
                        foreach ( flt1 in fntNuevas.Letter[CopyCode].FontBulges)
                        {
                            letra.FontBulges.Add(flt3);
                            foreach ( flt2 in flt1)
                            {
                                flt3.Add(flt2);
                            }
                        }

                    }
                        flt1 = new float[];
                        fltb = new float[];

                        letra.FontGlyps.Add(flt1);
                        letra.FontBulges.Add(fltb);

                        sPuntos = Split(sDAta, ");");

                        foreach ( sCoord in sPuntos)
                        {
                            sVert = Split(sCoord, ",");
                            BulgeAdded = false;
                            foreach ( aVert in sVert)
                            {
                                p1 = InStr(avert, "A");
                                if ( p1 > 0 )
                                {

                                     // Try letra.FontBulges.Add(CFloat(Mid$(aVert, p1 + 1)))
                                    Try fltb.Add(CSingle(Mid$(aVert, p1 + 1)));
                                    BulgeAdded = true;

                                }

                                     // Try letra.FontGlyps.Add(CFloat(aVert))
                                    flt1.Add(CSingle(aVert));

                                }
                            }
                            if ( ! BulgeAdded ) fltb.Add(0);
                        }
                    }
                Wend; // fin de la letra
                fntNuevas.Letter.Add(letra, letra.Code);

            } // ignoro todos los comentarios

        }
        fntNuevas.FontName = Utils.FileWithoutExtension(sFilename);
        glFont.Add(fntNuevas, fntNuevas.FontName);
        ActualFont = fntNuevas;
        if ( fntNuevas.FontName == "unicode" ) UnicodeFont = fntNuevas;

        Console.WriteLine( ("LeIdas " + fntNuevas.Letter.Count + " letras en " + sFilename);
        lista.Add(fntNuevas.FontName);

    }

    return lista;

}

 // // Busca el codigo UTF y devuelve la posicion en el indice de letras
 // Public Function GetCodeIndex(LaFont As FontSt, UTFcode As int) As int
 //
 //   Dim i As int
 //
 //   If Not LaFont Then Return 0
 //
 //   For i = 0 To LaFont.Letter.Max
 //     If LaFont.Letter[i].Code = UTFcode Then Return i
 //   Next
 //
 //   // si estamos aca es porque no lo encontramos, buscamos en unicode
 //   For i = 0 To UnicodeFont.Letter.Max
 //     If UnicodeFont.Letter[i].Code = UTFcode Then Return -i
 //   Next
 //
 // End

 // Grafica un texto en el contexto actual de acuerdo a los parametros pasados
 // Debe estar definida la Font con nombre y altura
 //
 //Fast 
public static bool DrawText(string UTFstring, double posX, double posY, double angle= 0, double textH= 1, int colour= -14, double linewIdth= 1, double rectW= 0, double rectH= 0, int alignHoriz= 0, int alignVert= 0)
    {


    int i ;         
    int iii ;         
    int i2 ;         
    int UTFcode ;         
    int LetterIndex ;         
    double Xadvance ;         
     float[] fArcParams ;         
     float[] Glyps ;         
     float[] Bulges ;         
    float[][] TGlyps ;         
    float[][] TBulges ;         
    float Ang ;         
    float m1 ;         
    float m2 ;         
    float b ;         
    float bx ;         
    float by ;         
    float mx ;         
    float my ;         
    float ang1 ;         
    float lt ;         
    float alpha ;         
    int iBulge ;         

    gl.PushMatrix; // para evitar peleas, guardo la matriz de trnasformacion
    gl.Translatef(posX, posY, 0);
    gl.Rotatef(angle, 0, 0, 1); // en grados
    gl.Scalef(textH * FontScale, textH * FontScale, 1);
    if ( colour != -14 ) this.glColorRGB(colour);
    for ( i = 1; i <= String.Len(UTFstring); i + 1) // para cada letra
    {
        UTFcode = String.Code(UTFstring, i); // obtengo el UTF code
        if ( UTFcode == 32 ) // es un espacio
        {
            gl.Translatef(ActualFont.WordSpacing, 0, 0); // muevo el puntero a la siguiente posicion
        }
             // DEPRE LetterIndex = GetCodeIndex(ActualFont, UTFcode)                     // obtengo el indice de la letra
            if ( ActualFont.Letter.Exist(UTFcode) )
            {
                TGlyps = ActualFont.Letter[UTFcode].FontGlyps;
                TBulges = ActualFont.Letter[UTFcode].FontBulges;

            } // is unicode
            else if ( UnicodeFont.Letter.Exist(UTFcode) )
            {
                TGlyps = UnicodeFont.Letter[UTFcode].FontGlyps;
                TBulges = UnicodeFont.Letter[UTFcode].FontBulges;

            }
                 // descarto la letra
                Continue;

            }

             // con bulges
             //============================================================================
            iBulge = 0;

            foreach ( Glyps in TGlyps)
            {
                Bulges = TBulges[iBulge];

                for ( i2 = 0; i2 <= Glyps.Count / 2 - 2; i2 + 1)
                {

                     // no todos los tramos pueden tener bulges
                    if ( Abs(Bulges[i2 + 1]) > 0.001 )
                    {
                         // // FIXME: arc problem
                         // Continue
                        ang1 = Ang(Glyps[(i2 + 1) * 2] - Glyps[i2 * 2], Glyps[(i2 + 1) * 2 + 1] - Glyps[i2 * 2 + 1]); // angulo del tramo
                        Lt = puntos.distancia(Glyps[i2 * 2], Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);
                        if ( Lt == 0 ) Continue;
                        mx = (Glyps[(i2 + 1) * 2] + Glyps[i2 * 2]) / 2; // punto medio del tramo
                        my = (Glyps[(i2 + 1) * 2 + 1] + Glyps[i2 * 2 + 1]) / 2;
                        B = Bulges[i2 + 1] * Lt / 2;
                        if ( Bulges[i2 + 1] < 0 ) alpha = Math.PI / 2 } else { alpha = -Pi / 2;
                        bx = mx + B * Cos(ang1 + alpha); // Tercer punto del Bulge
                        by = my + B * Sin(ang1 + alpha);

                         // aqui podria usar una rutina de arco entre 3 puntos
                        fArcParams = puntos.Arc3Point(Glyps[i2 * 2], Glyps[i2 * 2 + 1], bx, by, Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);
                         // traslado el centro
                         //If (Bulges[i2 + 1] > 0) Then Swap fArcParams[3], fArcParams[4]

                        gl.Translatef(fArcParams[0], fArcParams[1], 0);

                        ARC(fArcParams[2], fArcParams[3], fArcParams[4],, colour);
                        gl.Translatef(-fArcParams[0], -fArcParams[1], 0);
                        fArcParams = Null;

                    } // dibujo la linea normalmente

                        drawLines([Glyps[i2 * 2], Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]], Colour);
                    }

                }
                for ( iii = 0; iii <= Glyps.Max; iii + 2) // calculo cuanto tiene que avanzar el puntero
                {
                    if ( Glyps[iii] > Xadvance ) Xadvance = Glyps[iii];
                }

                Inc iBulge;
            }
            gl.Translatef(Xadvance + ActualFont.LetterSpacing, 0, 0); // muevo el puntero a la siguiente posicion
             //Console.WriteLine( Xadvance
            Xadvance = 0;
             //================================================================================

        }
    }
    gl.PopMatrix;

}

 // Devuelve una poly con el texto en el contexto actual de acuerdo a los parametros pasados
 // Debe estar definida la Font con nombre y altura
 public  DrawTextPoly(UTFstring As String,  textH As double = 1, sRotationRad As float = 0, sItalicAngle As float = 0, fScaleX As double = 1) As double[];

    int i ;         
    int iii ;         
    int i2 ;         
    int UTFcode ;         
    int LetterIndex ;         
    double Xadvance ;         
    double xMax ;         
     float[] fArcParams ;         
     float[] Glyps ;         
     float[] Bulges ;         
    float[][] TGlyps ;         
    float[][] TBulges ;         
    float Ang ;         
    float m1 ;         
    float m2 ;         
    float b ;         
    float bx ;         
    float by ;         
    float mx ;         
    float my ;         
    float ang1 ;         
    float lt ;         
    int iBulge ;         

    double dX ;          // donde tengo el cursor
    double dY ;         
    double alpha ;         
     Float[] flxArc ;         
     Float[] flxGlyps ;         
     Float[] flxAnswer ;         

     //SelectFont("romant")
     // gl.Scalef(textH * FontScale, textH * FontScale, 1)
    for ( i = 1; i <= String.Len(UTFstring); i + 1) // para cada letra
    {
        UTFcode = String.Code(UTFstring, i); // obtengo el UTF code

        if ( UTFcode == 32 ) // es un espacio
        {
            Xadvance += ActualFont.WordSpacing; // muevo el puntero a la siguiente posicion

        }
             // DEPRE LetterIndex = GetCodeIndex(ActualFont, UTFcode)                     // obtengo el indice de la letra
            if ( ActualFont.Letter.Exist(UTFcode) )
            {
                TGlyps = ActualFont.Letter[UTFcode].FontGlyps;
                TBulges = ActualFont.Letter[UTFcode].FontBulges;

            } // is unicode
            else if ( UnicodeFont.Letter.Exist(UTFcode) )
            {
                TGlyps = UnicodeFont.Letter[UTFcode].FontGlyps;
                TBulges = UnicodeFont.Letter[UTFcode].FontBulges;

            }
                 // descarto la letra
                Continue;

            }

             // con bulges
             //============================================================================
            iBulge = 0;

            foreach ( Glyps in TGlyps)
            {
                Bulges = TBulges[iBulge];

                for ( i2 = 0; i2 <= Glyps.Count / 2 - 2; i2 + 1)
                {

                     // no todos los tramos pueden tener bulges
                    if ( Abs(Bulges[i2 + 1]) > 0.001 )
                    {
                         // // FIXME: arc problem
                         // Continue
                        ang1 = Ang(Glyps[(i2 + 1) * 2] - Glyps[i2 * 2], Glyps[(i2 + 1) * 2 + 1] - Glyps[i2 * 2 + 1]); // angulo del tramo
                        Lt = puntos.distancia(Glyps[i2 * 2], Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);
                        if ( Lt == 0 ) Continue;
                        mx = (Glyps[(i2 + 1) * 2] + Glyps[i2 * 2]) / 2; // punto medio del tramo
                        my = (Glyps[(i2 + 1) * 2 + 1] + Glyps[i2 * 2 + 1]) / 2;
                        B = Bulges[i2 + 1] * Lt / 2;
                        if ( Bulges[i2 + 1] < 0 ) alpha = Math.PI / 2 } else { alpha = -Pi / 2;
                        bx = mx + B * Cos(ang1 + alpha); // Tercer punto del Bulge
                        by = my + B * Sin(ang1 + alpha);

                         // aqui podria usar una rutina de arco entre 3 puntos

                        fArcParams = puntos.Arc3Point(Glyps[i2 * 2], Glyps[i2 * 2 + 1], bx, by, Glyps[(i2 + 1) * 2], Glyps[(i2 + 1) * 2 + 1]);

                        flxArc = ArcPoly(fArcParams[0] + Xadvance, fArcParams[1], fArcParams[2], fArcParams[3], fArcParams[4], Math.PI / 16);

                        flxAnswer.Insert(flxArc);

                        fArcParams.Clear;
                        flxArc.Clear;

                    } // dibujo la linea normalmente

                        flxAnswer.Insert([Glyps[i2 * 2] + Xadvance, Glyps[i2 * 2 + 1], Glyps[(i2 + 1) * 2] + Xadvance, Glyps[(i2 + 1) * 2 + 1]]);
                    }

                }
                for ( iii = 0; iii <= Glyps.Max; iii + 2) // calculo cuanto tiene que avanzar el puntero
                {
                    if ( Glyps[iii] > xMax ) xMax = Glyps[iii];
                }

                Inc iBulge;
            }
            Xadvance += xMax + ActualFont.LetterSpacing; // muevo el puntero a la siguiente posicion

            xMax = 0;
             //================================================================================

        }
    }

    puntos.Scale(flxAnswer, textH * FontScale * fScaleX, textH * FontScale);

    if ( sItalicAngle != 0 )
    {

        for ( iii = 0; iii <= flxAnswer.Max - 1; iii + 2)
        {
            flxAnswer[iii] += flxAnswer[iii + 1] * Sin(Rad(sItalicAngle));
        }

    }
    if ( sRotationRad != 0 ) puntos.Rotate(flxAnswer, sRotationRad);
    return flxAnswer;

}

 // AlingHoriz : 0=Rigth, 1=Center, 2=Left
 // AlingVert: 0=Top, 1=Center, 2=Bottom
public static bool DrawText2(string UTFstring, double posX, double posY, double angle= 0, double textH= 1, int colour= -14, int BackColour= -1, double linewIdth= 1, bool italic= false, double rectW= 0, double rectH= 0, int alignHoriz= 0, int alignVert= 0)
    {


     Float[] flxText ;         
     Float[] tRect ;         
    float sItalicAngle ;         
    double tX ;         
    double tY ;         
    double factorX ;         
    double factorY ;         
    double fBorderExtension = 3;

    if ( italic ) sItalicAngle = 20;

    flxText = DrawTextPoly(UTFstring, textH, angle, sItalicAngle);
    tRect = puntos.Limits(flxText);

     // veo si tengo que comprimir en un ractangulo
    if ( (rectH > 0) && (rectW > 0) )
    {

        factorX = rectW / (tRect[2] - tRect[0]);
        factorY = rectH / (tRect[3] - tRect[1]);

        puntos.Scale(flxText, factorX, factorY);

    }

        rectH = tRect[3] - tRect[1];
        rectW = tRect[2] - tRect[0];
    }

    if ( alignHoriz == 1 ) tX = -rectW / 2;
    if ( alignHoriz == 2 ) tX = -rectW;

    if ( alignVert == 1 ) tY = -rectH / 2;
    if ( alignVert == 2 ) tY = -rectH;

    puntos.Translate(flxText, tx, ty);

    gl.MatrixMode(gl.PROJECTION);
    gl.PushMatrix;

    gl.LoadIdentity();

    Gl.Ortho(0, fmain.gestru.w, 0, fmain.gestru.h, 0, 1);

    gl.MatrixMode(gl.MODELVIEW);
    gl.PushMatrix;
    gl.LoadIdentity();
    gl.Translatef(posX, posY, 0);
    DrawLines(flxText, colour, linewIdth);
    Rectangle2D(tx - fBorderExtension, ty - fBorderExtension, rectW + fBorderExtension * 2, rectH + fBorderExtension * 2, BackColour,,,,,,, 0);

    gl.PopMatrix;
    gl.MatrixMode(gl.PROJECTION);
    gl.PopMatrix;

}

 // AlingHoriz : 0=Rigth, 1=Center, 2=Left
 // AlingVert: 0=Top, 1=Center, 2=Bottom
public static bool DrawText3(string UTFstring, double posX, double posY, double posZ, double angle= 0, double textH= 1, int colour= -14, int BackColour= -1, double linewIdth= 1, bool italic= false, double rectW= 0, double rectH= 0, int alignHoriz= 0, int alignVert= 0)
    {


     Float[] flxText ;         
     Float[] tRect ;         
    float sItalicAngle ;         
    double tX ;         
    double tY ;         
    double factorX ;         
    double factorY ;         
    double fBorderExtension = 3;

    if ( italic ) sItalicAngle = 20;

    flxText = DrawTextPoly(UTFstring, textH, angle, sItalicAngle);
    tRect = puntos.Limits(flxText);

     // veo si tengo que comprimir en un ractangulo
    if ( (rectH > 0) && (rectW > 0) )
    {

        factorX = rectW / (tRect[2] - tRect[0]);
        factorY = rectH / (tRect[3] - tRect[1]);

        puntos.Scale(flxText, factorX, factorY);

    }

        rectH = tRect[3] - tRect[1];
        rectW = tRect[2] - tRect[0];
    }

    if ( alignHoriz == 1 ) tX = -rectW / 2;
    if ( alignHoriz == 2 ) tX = -rectW;

    if ( alignVert == 1 ) tY = -rectH / 2;
    if ( alignVert == 2 ) tY = -rectH;

    puntos.Translate(flxText, tx, ty);

     // gl.MatrixMode(gl.PROJECTION)
     // gl.PushMatrix
     //
     // gl.LoadIdentity()
     //
     // Gl.Ortho(0, fmain.gestru.w, 0, fmain.gestru.h, 0, 1)
     //
     // gl.MatrixMode(gl.MODELVIEW)
     // gl.PushMatrix
     // gl.LoadIdentity()
    gl.Translatef(posX, posY, 0);
    DrawLines(flxText, colour, linewIdth);
    Rectangle2D(tx - fBorderExtension, ty - fBorderExtension, rectW + fBorderExtension * 2, rectH + fBorderExtension * 2, BackColour,,,,,,, 0);

     // gl.PopMatrix
     // gl.MatrixMode(gl.PROJECTION)
     // gl.PopMatrix

}

 // devuelve un rectangulo que contiene al texto
 // [ancho,alto]
 public  TextExtends(UTFstring As String,  textH As double = 1, sRotationRad As float = 0, sItalicAngle As float = 0) As double[];

     Float[] flxText ;         
     Float[] tRect ;         

    flxText = DrawTextPoly(UTFstring, textH, sRotationRad, sItalicAngle);
    tRect = puntos.Limits(flxText);
     //tRect[1] *= textH * FontScale * 1.2

    return tRect;

}

 // Lee todas las texturas del directorio provisto y devuelve un listado con sus nombres
public static string[] LoadTextures(string DirPath)
    {


    string sFilename ;         
    File fFile ;         
    string sCoord ;         
     string[] Lista ;         

    int iTexture = 0;
    TextureSt newTexture ;         

    hText = Gl.GenTextures(1);

    foreach ( sFilename in Dir(DirPath, "*.png"))
    {

        newTexture = new TextureSt;

        glTextures.Add(newTexture);

        newTexture.FileName = Left$(sFilename, -4); // agrego el nombre de la textura a la lista que voy a retornar

        lista.Add(newTexture.FileName);

        newTexture.hImage = Image.Load(DirPath &/ sFilename); // cargo la imagen en memoria

        Gl.TexImage2D(newTexture.hImage); // genero un objeto OpenGL

        Gl.TexParameteri(Gl.TEXTURE_2D, Gl.TEXTURE_MIN_FILTER, Gl.NEAREST); // parametros basicos opengl

        Gl.TexParameteri(Gl.TEXTURE_2D, Gl.TEXTURE_MAG_FILTER, Gl.NEAREST); // parametros basicos opengl

        gl.BindTexture(gl.TEXTURE_2D, hText[iTexture]); // enlazo la textura a una handle

        newTexture.Id = hText[iTexture];

        Inc iTexture;
        Break;
    }

    Console.WriteLine( ("LeIdas " + iTexture + " texturas en " + sFilename);

    return lista;

}

 // Dibuja un triangulo con una textura ya cargada

public static void TexturedTriangle2D(double x1, double y1, double x2, double y2, double x3, double y3, int TextureId, double Scale)
    {


    Gl.TexImage2D(glTextures[TextureId].hImage); // genero un objeto OpenGL
    gl.BindTexture(gl.TEXTURE_2D, hText[TextureId]); // enlazo la textura a una handle
    gl.begin(gl.TRIANGLES);

    gl.TexCoord2f(0, 0);

    gl.Vertex2f(x1, y1);

    gl.TexCoord2f((x2 - x1) / scale / 1000, (y2 - y1) / scale / 1000);

    gl.Vertex2f(x2, y2);

    gl.TexCoord2f((x3 - x1) / scale / 1000, (y3 - y1) / scale / 1000);

    gl.Vertex2f(x3, y3);

    gl.End;

}

public static int createVBO(Pointer data, int dataSizeBytes, int usage)
    {


    int e = 0;  // 0 Is Reserved, glGenBuffersARB()will Return non - zero id If success
    int Id = 0; 
     int[1] iParams ;         

    glGenBuffers(1, VarPtr(Id)); //Create a vbo
    glBindBuffer(ARRAY_BUFFER, Id); //activate vbo id To Use
    glBufferData(ARRAY_BUFFER, dataSizeBytes, data, usage); //upload data To video card
    e = glGetError();

     // check data size In VBO Is Same As Input array, If Not Return 0 And delete VBO
    glGetBufferParameteriv(ARRAY_BUFFER, BUFFER_SIZE, iParams.Data);

    if ( (dataSizeBytes != iParams[0]) )
    {
        glDeleteBuffers(1, VarPtr(Id));
        Id = 0;
        Console.WriteLine( "[createVBO()] Data size is mismatch with input array");
        Console.WriteLine( "Bufferdata:" + Hex(e);
         // Else
         //     Console.WriteLine( "[createVBO()] Buffer created OK"
    }

    return Id; // Return VBO id

}

public static bool CheckExtension(string sExtension)
    {


    return (InStr(LCase(GLx.glGetString(EXTENSIONS)), LCase(sExtension)) > 0);

}

 //El contexto debe estar creado para poder usar los Shaders
public static int LoadShader(string sVertexShaderFile, string sFragmentShaderFile)
    {


    string sShader ;         
    string sInput ;         
    File f ;         
    int iShaderProgram ;         
    int iVerShaderID ;         
    int iFraShaderID ;         
     // verificamos si el sistema soporta shaders
     // If Not gl.CheckExtensions("GL_ARB_vertex_program") Then
     //   Console.WriteLine( "No se pueden cargar Shaders"
     //
     //   Return 0
     // End If

    if ( ! Exist(sVertexShaderFile) )
    {
        Console .WriteLine( "No existe el Vertex Shader");

        return 0;
    }
    if ( ! Exist(sFragmentShaderFile) )
    {
        Console .WriteLine( "No existe el Vertex Shader");

        return 0;
    }
    f = Open sVertexShaderFile for ( Input;
    do {
        #f, sInput= Console.ReadLine();
  
        sShader &= sInput + gb.CrLf;

    }

    Close #f;

     // La siguiente linea cuelga el programa si no hay contexto
    iVerShaderID = gl.CreateShader(gl.VERTEX_SHADER);
    if ( iVerShaderID == 0 ) Stop;
     // // set the source code
    Gl.ShaderSource(iVerShaderID, sShader);
     // // compile
    gl.CompileShader(iVerShaderID);

    sShader = "");
    f = Open sFragmentShaderFile for ( Input;
    do {
        #f, sInput= Console.ReadLine();

        sShader &= sInput + gb.CrLf;

    }

    Close #f;

    iFraShaderID = gl.CreateShader(gl.FRAGMENT_SHADER);
    if ( iFRaShaderID == 0 ) Stop;
     // // set the source code
    Gl.ShaderSource(iFraShaderID, sShader);
     // // compile
    gl.CompileShader(iFraShaderID);

    ishaderProgram = gl.CreateProgram();
    gl.AttachShader(ishaderProgram, iVerShaderID);
    gl.AttachShader(ishaderProgram, iFraShaderID);
    gl.LinkProgram(ishaderProgram);

     //
    Console.WriteLine( gl.GetShaderInfoLog(ishaderProgram));
     //
     // // una vez complilados, no se necesitan mas
     //
    gl.DeleteShader(iVerShaderID);
    gl.DeleteShader(iFraShaderID);

    return iShaderProgram;

}

public static void txtRendering2D(string texto, double x, double y, float Altura= 12, long _color= Color.Blue, long _BackColor= -1, int centradoH= 0, int centradoV= 0)
    {


     // this works in a Orthogonal projection

    RectF rectangulo ;         

     Image imagen ;         
     Punto3d p ;         

    imagen.Resize(200, 200); //so Paint.Begin gives no error

    Paint.Begin(imagen);

    Paint.Font.Size = Altura;

    Paint.Brush = Paint.Color(_color);

    rectangulo = Paint.TextSize(texto);

    Paint.End;

    rectangulo.Height /= 1.25; // correct the innecesary extra height

     //this can//t go into the Paint loop

    imagen.resize(rectangulo.Width, rectangulo.Height);

    if ( _backcolor > 0 )
    {

        imagen.Fill(_BackColor);

    }

         // until I know how to make it transparente, it goes like this

        imagen.Fill(Color.White);

    }

    EscalaGL = 1;

    Paint.Begin(imagen);

    Paint.Font.Size = Altura;

    Paint.Brush = Paint.Color(_color);

    Paint.Text(texto, 0, rectangulo.Height * 0.85); //locate the text at the bottom + 15% so it//s vertically centered in the box

    Paint.Fill;

    Paint.End;

     //imagen.Save("imagen.png")  // this is to check the Paint worked (and works!) (re checckeado 02/06/16)

     // allignment to the 3D point p

    if ( centradoH == 1 ) x -= rectangulo.Width * EscalaGL;

    if ( centradoH == 2 ) x -= rectangulo.Width / 2 * EscalaGL;

    if ( centradoV == 1 ) y -= rectangulo.Height * EscalaGL;

    if ( centradoV == 2 ) y -= rectangulo.Height / 2 * EscalaGL;

    gl.MatrixMode(gl.PROJECTION);
    gl.PushMatrix;

    gl.LoadIdentity();

    Gl.Ortho(0, fmain.gestru.w, 0, fmain.gestru.h, 0, 1);

    gl.MatrixMode(gl.MODELVIEW);
    gl.PushMatrix;
    gl.LoadIdentity();

    Gl.TexImage2D(imagen);

    Gl.TexParameteri(Gl.TEXTURE_2D, Gl.TEXTURE_MIN_FILTER, Gl.NEAREST);

    Gl.TexParameteri(Gl.TEXTURE_2D, Gl.TEXTURE_MAG_FILTER, Gl.NEAREST);

    gl.BindTexture(gl.TEXTURE_2D, hText[0]);

    Gl.Begin(Gl.QUADS);

    gl.color3f(1, 1, 1);

    Gl.TexCoordf(0.0, 1.0); // Bottom Left OF The Texture AND Quad

    Gl.Vertex2f(x, y);

    Gl.TexCoordf(1.0, 1.0); // Bottom Right OF The Texture AND Quad

    Gl.Vertex2f(x + rectangulo.Width * EscalaGL, y);

    Gl.TexCoordf(1.0, 0.0); // Top Right OF The Texture AND Quad

    Gl.Vertex2f(x + rectangulo.Width * EscalaGL, y + rectangulo.Height * EscalaGL);

    Gl.TexCoordf(0.0, 0.0); // Top Left OF The Texture AND Quad

    Gl.Vertex2f(x, y + rectangulo.Height * EscalaGL);

    Gl.End();

    gl.PopMatrix;
    gl.MatrixMode(gl.PROJECTION);
    gl.PopMatrix;

}

public static void DrawText3D(string texto, Punto3d pr, float Altura= 12, long _color= Color.Blue, long _BackColor= -1, int centradoH= 0, int centradoV= 0)
    {


    double x ;         
    double y ;         
    double z ;         

    glx.Get2DpointFrom3Dworld(pr, ByRef x, ByRef y, ByRef z);

    if ( z <= 1 ) DrawText2(texto, x, y, 0, altura, _color, _backcolor,,,,, centradoH, centradoV);

}

 // Public Sub LucesOn()
 //
 //     Gl.Lightfv(Gl.LIGHT0, Gl.AMBIENT_AND_DIFFUSE, [1.0, 1.0, 1.0, 0.5])
 //
 //     Gl.Lightfv(Gl.LIGHT0, Gl.POSITION, [GLCam.camera.Position.x, GLCam.camera.Position.y, -GLCam.camera.Position.z, 1])
 //
 //     Gl.Enable(Gl.LIGHTING)
 //     Gl.Enable(Gl.LIGHT0)
 //
 //     Gl.Enable(Gl.NORMALIZE) // esto sirve para normalizar los vectores normales , o sea que sean de largo = 1
 //
 // End
 //
 // Public Sub LucesOff()
 //
 //     Gl.Disable(Gl.LIGHTING)
 //     Gl.disable(Gl.LIGHT0)
 //
 //     Gl.disable(Gl.NORMALIZE) // esto sirve para normalizar los vectores normales , o sea que sean de largo = 1
 //
 // End Sub

public static void GLQuadColor4F(Punto3d p1, Punto3d p2, Punto3d p3, Punto3d p4, int c1, int c2, int c3, int c4)
    {


     // Quad esta obsoleto , reemplazo por dos triangulos

    gl.begin(gl.TRIANGLES);

    Vertex3D(p1, c1);
    Vertex3D(p2, c2);
    Vertex3D(p3, c3);
    gl.End;

    gl.begin(gl.TRIANGLEs);
    Vertex3D(p1, c1);
    Vertex3D(p3, c3);
    Vertex3D(p4, c4);

    gl.end;

}

}