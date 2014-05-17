float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float xAmbient;


float4x4 World;
float4x4 LightView;
float4x4 LightProjection;

float4x4 View;
float4x4 Projection;

float4x4 MatTexture;
bool Model;
float2 PCFSamples[9];
float depthBias = 0.001;

float3 xLightPos;
float xLightPower;
float4x4 xLightsWorldViewProjection;
float4x4 xWorldViewProjection;

Texture xShadowMap;
texture shadowTexture;

//sampler ShadowMapSampler = sampler_state { texture = <xShadowMap>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = clamp; AddressV = clamp; };
sampler ShadowSampler = sampler_state
{
	Texture = (shadowTexture);
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Clamp;
	AddressV = Clamp;
};


struct SMapVertexToPixel
{
	float4 Position     : POSITION;
	float4 Position2D    : TEXCOORD0;
};

struct SMapPixelToFrame
{
	float4 Color : COLOR0;
};


struct SSceneVertexToPixel
{
	float4 Position             : POSITION;
	float4 Pos2DAsSeenByLight    : TEXCOORD0;

	float2 TexCoords            : TEXCOORD1;
	float3 Normal                : TEXCOORD2;
	float4 Position3D            : TEXCOORD3;

};

struct SScenePixelToFrame
{
	float4 Color : COLOR0;
};


float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
		return dot(-lightDir, normal);
}

SMapVertexToPixel ShadowMapVertexShader(float4 inPos : POSITION)
{
	SMapVertexToPixel Output = (SMapVertexToPixel)0;

	Output.Position = mul(inPos, xLightsWorldViewProjection);
	Output.Position2D = Output.Position;

	return Output;
}

SMapPixelToFrame ShadowMapPixelShader(SMapVertexToPixel PSIn)
{
	SMapPixelToFrame Output = (SMapPixelToFrame)0;

	Output.Color = PSIn.Position2D.z / PSIn.Position2D.w;

	return Output;
}


technique ShadowMap
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowMapVertexShader();
		PixelShader = compile ps_2_0 ShadowMapPixelShader();
	}
}

/////////////////////////////////////////////////////////////////////////////////////////////////
SSceneVertexToPixel ShadowedSceneVertexShader(float4 inPos : POSITION)
{
	SSceneVertexToPixel Output = (SSceneVertexToPixel)0;

	Output.Position = mul(inPos, xWorldViewProjection);
	Output.Pos2DAsSeenByLight = mul(inPos, xLightsWorldViewProjection);
	return Output;
}

SScenePixelToFrame ShadowedScenePixelShader(SSceneVertexToPixel PSIn)
{
	SScenePixelToFrame Output = (SScenePixelToFrame)0;

	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = PSIn.Pos2DAsSeenByLight.x / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;
	ProjectedTexCoords[1] = -PSIn.Pos2DAsSeenByLight.y / PSIn.Pos2DAsSeenByLight.w / 2.0f + 0.5f;

	Output.Color = tex2D(ShadowSampler, ProjectedTexCoords);

	return Output;
}

technique ShadowedScene
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ShadowedSceneVertexShader();
		PixelShader = compile ps_2_0 ShadowedScenePixelShader();
	}
}
/////////////////////////////////////////////////////////////////////////////////
//SC oznacza shadow casters


struct VertexShaderInputSC
{
	float4 Position : POSITION0;
};

struct VertexShaderOutputSC
{
	float4 Position : POSITION0;
	float Depth : TEXCOORD0;
};

VertexShaderOutputSC VertexShaderFunctionSC(VertexShaderInputSC input)
{
	VertexShaderOutputSC output;

	float4 worldPosition = mul(input.Position, World);
		float4 viewPosition = mul(worldPosition, LightView);
		output.Position = mul(viewPosition, LightProjection);
	output.Depth = output.Position.z;
	return output;
}

float4 PixelShaderFunctionSC(VertexShaderOutputSC input) : COLOR0
{
	return float4(input.Depth, input.Depth, input.Depth, 1);
}

technique Technique1
{
	pass Pass1
	{
		// TODO: set renderstates here.

		VertexShader = compile vs_2_0 VertexShaderFunctionSC();
		PixelShader = compile ps_2_0 PixelShaderFunctionSC();
	}
}
