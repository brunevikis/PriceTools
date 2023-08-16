    
	<!--  place after <vstav3:update enabled="true" /> on Compass.DecompTools.dll.manifest --> Necessário para habilitar as funções do menu de contexto do windows
    cd C:\Development\Publish\Application Files\DecompTools_1_5_0_37


	<vstav3:postActions>
      <vstav3:postAction>
        <vstav3:entryPoint class="DeployActions.DeployActions">
          <assemblyIdentity name="DeployActions" version="1.0.0.0" language="neutral" processorArchitecture="msil" />
        </vstav3:entryPoint>
        <vstav3:postActionData>
        </vstav3:postActionData>  
      </vstav3:postAction>
    </vstav3:postActions>  


mage -sign DecompTools.dll.manifest -certfile ../../DecompTools_11_TemporaryKey.pfx -pub Compass -Password enercore
mage -update ../../DecompTools.vsto -appmanifest DecompTools.dll.manifest -certfile ../../DecompTools_11_TemporaryKey.pfx -pub Compass -Password enercore
copy /Y ..\..\DecompTools.vsto .\

Copiar a pasta C:\Development\Publish\Application Files\DecompTools_1_5_0_35 para H:\TI - Sistemas\UAT\PricingExcelTools\pub\Application Files
	--Copiar o arquivo H:\TI - Sistemas\UAT\PricingExcelTools\pub\Application Files\DecompTools_1_5_0_35\DecompTools.vsto para H:\TI - Sistemas\UAT\PricingExcelTools\pub
