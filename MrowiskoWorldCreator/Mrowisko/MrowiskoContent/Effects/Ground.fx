// HLSL to simply sample from a texture

// Input parameters.
float4x4 View;
float4x4 Projection;

texture Ground;
sampler GroundSampler = sampler_state
{
	Texture = (Ground);

	MinFilter = LINEAR;
	MagFilter = LINEAR;
	MipFilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};
texture GroundText0;
sampler GroundText0Sampler = sampler_state
{
	Texture = (GroundText0);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};

texture GroundText1;
sampler GroundText1Sampler = sampler_state
{
	Texture = (GroundText1);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};

texture GroundText2;
sampler GroundText2Sampler = sampler_state
{
	Texture = (GroundText2);

	MinFilter = Linear;
	MagFilter = Linear;
	MipFilter = Linear;
	AddressU = wrap;
	AddressV = wrap;
};
// Vertex shader input structure.
struct VS_INPUT
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

// Vertex shader output structure.
struct VS_OUTPUT
{
	float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

// Vertex shader program.
VS_OUTPUT VertexShader2(VS_INPUT input)
{
	VS_OUTPUT output;

	//generate the view-projection matrix
	float4x4 vp = mul(View, Projection);
		output.Position = mul(input.Position, vp);

	output.TexCoord = input.TexCoord;

	return output;
}

float4 PixelShader2(VS_OUTPUT input) : COLOR
{
	float4 groundSample = tex2D(GroundSampler, input.TexCoord/100);

	float4 colour = float4(0, 0, 0, 1);
	colour += tex2D(GroundText0Sampler, input.TexCoord * 1) * groundSample.r;
	colour += tex2D(GroundText1Sampler, input.TexCoord * 1) * groundSample.g;
	colour += tex2D(GroundText2Sampler, input.TexCoord * 1) * groundSample.b;

	return colour;
}

technique Terrain
{
	pass Main
	{
		VertexShader = compile vs_2_0 VertexShader2();
		PixelShader = compile ps_2_0 PixelShader2();
	}
}