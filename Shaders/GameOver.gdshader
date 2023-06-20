shader_type canvas_item;
uniform sampler2D Screen_texture :  hint_screen_texture,filter_nearest_mipmap;
uniform float lod: hint_range(0.0, 5) = 0.0;
uniform float blend: hint_range(0.0, 20);
void fragment() {
	COLOR = mix(textureLod(Screen_texture,SCREEN_UV,blend), vec4(2,0,0,2),lod);
	
}
