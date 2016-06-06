using Canducci.Zip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for wscep
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class wscep : System.Web.Services.WebService
{

    // método construtor padrão do web service
    public wscep() { }

    [WebMethod] // anotação que faz com que este método seja reconhecido no web service
    public ZipCodeInfo searchByCep(string cep) // searchByCep é um método do web service que iremos utilizar para fazer a pesquisa pelo cep informado como parâmetro.
    {
        try
        {
            //Observação
            //Formato válido para o CEP: 01414000 ou 01414-000 ou 01.414-000

            ZipCodeLoad zipLoad = new ZipCodeLoad(); // instanciando o objeto de um serviço externo de cep
            ZipCodeInfo zipCodeInfo = null; // criando variável e atribuindo um valor nulo para ela

            ZipCode zipCode = null; // criando variável e atribuindo um valor nulo para ela
            if (ZipCode.TryParse(cep, out zipCode)) // converte o cep e atribui seu valor ao objeto zipCode criado anteriormente
            {
                zipCodeInfo = zipLoad.Find(zipCode); // faz o consumo do serviço para retornar um endereço que corresponda ao cep informado
            }

            return zipCodeInfo; // retorna o endereço encontrado
        }
        catch (ZipCodeException ex) // em caso de algum erro no código acima, uma exceção é lançada e o erro é retornado, em forma de xml
        {
            throw ex;
        }
    }

    [WebMethod] // anotação que faz com que este método seja reconhecido no web service
    public ZipCodeInfo[] searchByAddress(int paramUf, string paramCity, string paramAddress) // searchByAddress é um método do web service que iremos utilizar para fazer a pesquisa pelo uf, cidade e endereço informados como parâmetro.
    {
        try
        {
            //Observações
            //Cidade no minimo 3 letras
            //Endereço no minimo 3 letras

            ZipCodeLoad zipLoad = new ZipCodeLoad(); // instanciando o objeto de um serviço externo de cep
            ZipCodeInfo[] zipCodeInfos = null; // criando a variável de retorno, que é uma lista, mas atribuímos um valor nulo, por enquanto

            // criamos uma variável uf para identificar de qual uf iremos fazer a pesquisa, de acordo com a uf informada
            ZipCodeUf uf = (ZipCodeUf)Enum.GetValues(typeof(ZipCodeUf)).GetValue(paramUf);

            AddressCode addressCode = null; // criamos uma variável e atribuímos o valor nulo pra ela
            if (AddressCode.TryParse(uf, paramCity, paramAddress, out addressCode)) // convertemos nossos parâmetros em um endereço válido e atribuímos a variável criada anteriormente
            {
                zipCodeInfos = zipLoad.Address(addressCode); // consumimos o serviço que faz a pesquisa do endereço e retornamos seu resultado a variável de lista que criamos anteriormente
            }

            return zipCodeInfos; // retornamos os resultados em forma de lista
        }
        catch (ZipCodeException ex) // em caso de erro no código acima este método é chamado e executado, fazendo com que seja retornado um erro, no formato xml
        {
            throw ex;
        }
    }

}
