float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float3 xCamPos;
float xAmbient;







float xOvercast;

bool Clipping;
float4 ClipPlane0;

Texture xTexture;

sampler TextureSampler = sampler_state { texture = <xTexture>; magfilter = LINEAR; minfilter = LINEAR; mipfilter = LINEAR; AddressU = wrap; AddressV = wrap; }; 


//------- Technique: Textured --------
struct TPixelToFrame
{
	float4 Color : COLOR0;
};
struct TVertexToPixel
{
	float4 Position     : POSITION;
	float4 Color        : COLOR0;
	float LightingFactor : TEXCOORD0;
	float2 TextureCoords: TEXCOORD1;
	float2 clipDistances: TEXCOORD2;
};
TVertexToPixel TexturedVS(float4 inPos : POSITION, float3 inNormal : NORMAL, float2 inTexCoords : TEXCOORD0)
{
	TVertexToPixel Output = (TVertexToPixel)0;
	float4x4 preViewProjection = mul(xView, xProjection);
		float4x4 preWorldViewProjection = mul(xWorld, preViewProjection);

		Output.Position = mul(inPos, preWorldViewProjection);
	Output.TextureCoords = inTexCoords;

	float3 Normal = normalize(mul(normalize(inNormal), xWorld));
		Output.LightingFactor = 1;

	Output.clipDistances = dot(inPos, ClipPlane0); //MSS - Water Refactor added

	return Output;
}

TPixelToFrame TexturedPS(TVertexToPixel PSIn)
{
	TPixelToFrame Output = (TPixelToFrame)0;

	Output.Color = tex2D(TextureSampler, PSIn.TextureCoords);
	Output.Color.rgb *= saturate(PSIn.LightingFactor + xAmbient);

	if (Clipping)  clip(PSIn.clipDistances);  //MSS - Water Refactor added


	return Output;
}



technique Textured
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 TexturedVS();
		PixelShader = compile ps_2_0 TexturedPS();
	}
}