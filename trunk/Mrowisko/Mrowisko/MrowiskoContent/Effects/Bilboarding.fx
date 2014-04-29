float4x4 xProjection;
float4x4 xWorld;
float3 xCamPos;
float3 xAllowedRotDir;
float4x4 xView;
int xScale;
float xTime;

float xAmbient;
bool xEnableLighting;
float3 xLightPos;
float xLightPower;
Texture xBillboardTexture;
sampler textureSampler = sampler_state { texture = <xBillboardTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; };

//------- Technique: Bilboarding --------


struct BBVertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
	float4 Position3D        : TEXCOORD1;
	float3 Normal            : TEXCOORD2;


};
struct BBPixelToFrame
{
	float4 Color 	: COLOR0;
};

//------- Technique: CylBillboard --------
BBVertexToPixel CylBillboardVS(float3 inPos: POSITION0, float2 inTexCoord : TEXCOORD0, float3 inNormal : NORMAL)
{
	BBVertexToPixel Output = (BBVertexToPixel)0;

	float3 center = mul(inPos, xWorld);
		float3 eyeVector = center - xCamPos;
		int scaling = xScale;
	float3 upVector = xAllowedRotDir;
		upVector = normalize(upVector);
	float3 sideVector = cross(eyeVector, upVector);
		sideVector = normalize(sideVector);

	float3 finalPosition = center;
		

		finalPosition += ((inTexCoord.x - 0.5f)*(xScale / 30))*sideVector*xScale;
	finalPosition += ((1.5f - inTexCoord.y*1.5f)*(xScale / 30))*upVector*xScale;
	float4 finalPosition4 = float4(finalPosition, 1);

		float4x4 preViewProjection = mul(xView, xProjection);
		Output.Position = mul(finalPosition4, preViewProjection);
	Output.Normal = mul(normalize(inNormal), xWorld);

	Output.TexCoord = inTexCoord;
	Output.Position3D = mul(inPos, xWorld);

	return Output;
}
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
		return dot(-lightDir, normal);
}
BBPixelToFrame BillboardPS(BBVertexToPixel PSIn) : COLOR0
{
	
	float diffuseLightingFactor = 0.3f;
	if (xEnableLighting)
	{

		diffuseLightingFactor = DotProduct((xLightPos.x*sin(radians(xTime)), abs(xLightPos.y*sin(radians(xTime))), xLightPos.z*sin(radians(xTime))), PSIn.Position3D, PSIn.Normal);
		diffuseLightingFactor = saturate(diffuseLightingFactor);
		diffuseLightingFactor *= xLightPower;
	}
	
	
	
	
	
	BBPixelToFrame Output = (BBPixelToFrame)0;
	Output.Color = tex2D(textureSampler, PSIn.TexCoord);

	clip(Output.Color.w - 0.7843f);
	Output.Color = tex2D(textureSampler, PSIn.TexCoord)*(diffuseLightingFactor + xAmbient);

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