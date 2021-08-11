    
	<!--  place after <vstav3:update enabled="true" /> on Compass.DecompTools.dll.manifest --> Necessário para habilitar as funções do menu de contexto do windows



	<vstav3:postActions>
      <vstav3:postAction>
        <vstav3:entryPoint class="DeployActions.DeployActions">
          <assemblyIdentity name="DeployActions" version="1.0.0.0" language="neutral" processorArchitecture="msil" />
        </vstav3:entryPoint>
        <vstav3:postActionData>
        </vstav3:postActionData>
      </vstav3:postAction>
    </vstav3:postActions>


mage -sign DecompTools.dll.manifest -certfile ../../DecompTools_4_TemporaryKey.pfx -pub Compass -Password compass
mage -update ../../DecompTools.vsto -appmanifest DecompTools.dll.manifest -certfile ../../DecompTools_4_TemporaryKey.pfx -pub Compass -Password compass
copy /Y ..\..\DecompTools.vsto .\
