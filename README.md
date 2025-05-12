# Sketch2D

Testes com uma classe para simular calculos de fisca, veja [SimuladorFisica.cs](Assets/Scripts/SimuladorFisica.cs).
Inicie-a no Start() de um MonoBehaviour, informando o Transform.

```cs
public class AnyGameObject : MonoBehaviour {

	public SimuladorFisica fisica;
	
    void Start() {

		fisica = new SimuladorFisica( transform );
		
    }
	
}
```



