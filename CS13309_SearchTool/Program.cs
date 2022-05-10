using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace CS13309_SearchTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length > 1)
            {
                if (args[0].Equals("retrieve"))
                {
                    for (int i = 1; i < args.Length; i++)
                    {
                        Console.WriteLine("Resultados encontrados para la palabra <" + args[i] + "> ...");
                        String palabraBuscar = args[i].ToLower();
                        buscarPalabra(palabraBuscar);
                        Console.WriteLine("------------------------");
                    }
                }
            }
        }

        public static void buscarPalabra(string palabraBuscada)
        {
            String lineaSiguiente;
            String[] postingNumbers = new string[50];
            List<String> listPosting = new List<string>();
            int numeroLinea = 0, resultadoBusquedaInicio = -1, resultadoBusquedaFinal = 0, contador = 0;

            
                string FileToRead = @"C:\\CS13309\\Tokens.txt";
                using (StreamReader ReaderObject = new StreamReader(FileToRead))
                {
                    string Line;
                    while ((Line = ReaderObject.ReadLine()) != null)
                    {
                        resultadoBusquedaInicio = encontrarPalabraEnTokens(palabraBuscada, Line);
                        
                        if (resultadoBusquedaInicio > 0)
                        {
                            lineaSiguiente = ReaderObject.ReadLine();
                            resultadoBusquedaFinal = Int32.Parse(lineaSiguiente.Substring(encontrarComas(lineaSiguiente)));

                            //Console.WriteLine(resultadoBusquedaFinal + "   " + resultadoBusquedaInicio);
                            listPosting = encontrarUbicacionEnPosting(resultadoBusquedaInicio, resultadoBusquedaFinal);
                            postingNumbers = listPosting.ToArray();
                            while (contador < listPosting.Count && contador < 10)
                        {
                                Console.WriteLine(contador + ".- " + compararPostingConDiccionario(postingNumbers[contador]));
                                contador++;
                            }
                            break;
                        }

                        numeroLinea++;
                    }
                }
            
        }

        public static int encontrarPalabraEnTokens(String palabraBusqueda, String linea) //le pasamos línea por línea para que compare los strings
        {
            for (int i = 0; i < palabraBusqueda.Length; i++)
            {
                if (!linea.ElementAt(i).Equals(palabraBusqueda.ElementAt(i)))
                {
                    return -1;
                }
            }
            int resultado = Int32.Parse(linea.Substring(encontrarComas(linea)));

            return resultado;
        }

        public static int encontrarComas(String linea)
        {
            char semicolon = '\u003B';
            int contadorComas = 0;

            for (int i = 0; i < linea.Length; i++)
            {
                if (linea.ElementAt(i).Equals(semicolon))
                {
                    contadorComas++;
                    if (contadorComas > 1)
                    {
                        i++;
                        return i;
                    }
                }
            }
            return -1;
        }

        public static List<String> encontrarUbicacionEnPosting(int comienzoCaracter, int terminoCaracter) //recorre el archivo posting hasta la linea donde se encuentra el caracter
        {
            String numeroArchivo = "";
            List<String> posicionesEnPosting = new List<string>();
            int contador = 0;
            char semicolon = '\u003B';
            int repeticiones = terminoCaracter - comienzoCaracter;
            double pesoAnterior = 0;

            try
            {
                string FileToRead = @"C:\\CS13309\\Posting.txt";
                using (StreamReader ReaderObject = new StreamReader(FileToRead))
                {
                    for (int i = 0; i < comienzoCaracter; i++)
                    {
                        ReaderObject.ReadLine();
                    }
                    
                    string Line;
                    while ((Line = ReaderObject.ReadLine()) != null && repeticiones > 0)
                    {
                        for (int i = 0; i < Line.Length; i++)
                        {
                            if (!Line.ElementAt(i).Equals(semicolon))
                            {
                                numeroArchivo = numeroArchivo.Insert(i, char.ToString(Line.ElementAt(i)));
                            }
                            else
                            {
                                if (Double.Parse(Line.Substring(i + 1)) > pesoAnterior && contador != 0)
                                {
                                    posicionesEnPosting.Insert(0, numeroArchivo);
                                }
                                else
                                {
                                    posicionesEnPosting.Add(numeroArchivo);
                                }

                                numeroArchivo = "";
                                pesoAnterior = Double.Parse(Line.Substring(i + 1));
                                break;
                            }
                        }

                        contador++;
                        repeticiones--;

                    }
                }

                return posicionesEnPosting;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public static string compararPostingConDiccionario(String ubicacionEncontrada)
        {
            Boolean nombreHTMLencontrado;

            try
            {
                string FileToRead = @"C:\\CS13309\\Diccionario.txt";
                using (StreamReader ReaderObject = new StreamReader(FileToRead))
                {
                    string Line;
                    while ((Line = ReaderObject.ReadLine()) != null)
                    {
                        nombreHTMLencontrado = true;

                        for (int i = 0; i < ubicacionEncontrada.Length; i++)
                        {
                            if (!Line.ElementAt(i).Equals(ubicacionEncontrada.ElementAt(i)))
                            {
                                nombreHTMLencontrado = false;
                                break;
                            }
                        }

                        if (nombreHTMLencontrado)
                        {
                            return Line.Substring(ubicacionEncontrada.Length + 1);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error en método compararPostingConDiccionario");
                throw;
            }

            return null;
        }
    }
}
