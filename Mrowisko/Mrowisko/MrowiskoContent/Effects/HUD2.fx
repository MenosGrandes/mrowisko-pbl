float4x4 xProjection;
float4x4 xWorld;
float3 xCamPos;
float3 xAllowedRotDir;
float4x4 xView;
int xScale;
float xScaleX;
float xAmbient;
Texture xBillboardTexture;
sampler textureSampler = sampler_state { texture = <xBillboardTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

//------- Technique: Bilboarding --------


struct BBVertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
};
struct BBPixelToFrame
{
	float4 Color 	: COLOR0;
};

//------- Technique: CylBillboard --------
BBVertexToPixel CylBillboardVS(float3 inPos: POSITION0, float2 inTexCoord : TEXCOORD0)
{
	BBVertexToPixel Output = (BBVertexToPixel)0;

	float3 center = mul(inPos, xWorld);
		float3 eyeVector = center - xCamPos;
		//int scaling = xScale;
		float3 upVector = xAllowedRotDir;
		upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector, upVector);
		sideVector = normalize(sideVector);

	float3 finalPosition = center;


		finalPosition += ((inTexCoord.x - 0.5f)*(xScale))*sideVector*xScale*xScaleX;
	finalPosition += ((1.5f - inTexCoord.y*1.5f)*(xScale))*upVector*xScale;
	float4 finalPosition4 = float4(finalPosition, 1);

		float4x4 preViewProjection = mul(xView, xProjection);
		Output.Position = mul(finalPosition4, preViewProjection);

	Output.TexCoord = inTexCoord;

	return Output;
}

BBPixelToFrame BillboardPS(BBVertexToPixel PSIn) : COLOR0
{
	BBPixelToFrame Output = (BBPixelToFrame)0;
	Output.Color = tex2D(textureSampler, PSIn.TexCoord);
	Output.Color += xAmbient;
	

	clip(Output.Color.w - 0.7843f);

	return Output;
}

technique CylBillboard
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 CylBillboardVS();
		PixelShader = compile ps_2_0 BillboardPS();
	}
}