shader_type canvas_item;
uniform sampler2D Screen_texture :  hint_screen_texture;
uniform float lod: hint_range(0.0, 5) = 0.0;
uniform float blend: hint_range(0.0, 20);
void fragment() {
	COLOR = mix(textureLod(Screen_texture,SCREEN_UV,blend), vec4(0,0,0,0),lod);
}

