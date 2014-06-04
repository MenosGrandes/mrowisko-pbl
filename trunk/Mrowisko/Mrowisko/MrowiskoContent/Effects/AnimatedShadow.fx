

float4x4 Bones[50];

float4x4 World;
float4x4 LightView;
float4x4 LightProjection;



struct VertexShaderInputSC
{
	float4 Position : POSITION0;
	float4 BoneIndices       : BLENDINDICES0;
	float4 BoneWeights       : BLENDWEIGHT0;
};

struct VertexShaderOutputSC
{
	float4 Position : POSITION0;
	float Depth : TEXCOORD0;
};

VertexShaderOutputSC VertexShaderFunctionSC(VertexShaderInputSC input)
{
	VertexShaderOutputSC output;

	float4x4 skinTransform = 0;

		skinTransform += Bones[input.BoneIndices.x] * input.BoneWeights.x;
	skinTransform += Bones[input.BoneIndices.y] * input.BoneWeights.y;
	skinTransform += Bones[input.BoneIndices.z] * input.BoneWeights.z;
	skinTransform += Bones[input.BoneIndices.w] * input.BoneWeights.w;

	float4 animWorldPos = mul(input.Position, skinTransform);
		float4 worldPosition = mul(animWorldPos, World);
		
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