shader_type canvas_item;

uniform sampler2D screen_texture : hint_screen_texture, repeat_disable, filter_nearest;

uniform sampler2D distance_curve;
uniform float intensity = 150.0;

void fragment() {
	// Called for every pixel the material is visible on.
	vec2 centered_uv = UV - vec2(0.5,0.5);
	float distance_from_center = abs(distance(vec2(0.0,0.0),centered_uv));
	float curve_distance_intensity = texture(distance_curve, vec2(distance_from_center)).r;

	
	//vec3 col = texture(screen_texture, SCREEN_UV).xyz;
	
	float blue_channel = texture(screen_texture, SCREEN_UV).b;
	float red_channel = texture(screen_texture, SCREEN_UV - centered_uv * curve_distance_intensity * SCREEN_PIXEL_SIZE * intensity).r;
	float green_channel = texture(screen_texture, SCREEN_UV - centered_uv * curve_distance_intensity * SCREEN_PIXEL_SIZE * intensity / 2.0).g;
	COLOR.rgb = vec3(red_channel,green_channel,blue_channel);
	
	//COLOR.xyz = col;
}
