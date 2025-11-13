# ToonNet

> **EN:** Token-Oriented Object Notation (TOON) — a lightweight, efficient notation for serializing and deserializing objects, optimized for AI API integrations.  
> **PT:** Notação de Objetos Orientada a Tokens (TOON) — uma notação leve e eficiente para serialização e desserialização de objetos, otimizada para integrações com APIs de IA.

[![NuGet](https://img.shields.io/nuget/v/ToonNet.svg?logo=nuget)](https://www.nuget.org/packages/ToonNet)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![.NET](https://img.shields.io/badge/.NET%20Standard-2.0-blue.svg)](https://learn.microsoft.com/dotnet/standard/net-standard)

---

![ToonNet Logo](icon.png)

## Overview / Visão Geral

**EN:**  
ToonNet is a .NET Standard 2.0 library that implements **TOON (Token-Oriented Object Notation)** — a compact textual representation of structured data, designed to reduce token cost in AI API interactions.

**PT:**  
ToonNet é uma biblioteca .NET Standard 2.0 que implementa o formato **TOON (Token-Oriented Object Notation)**, uma representação textual compacta de dados estruturados, criada para reduzir o custo de tokens em interações com APIs de IA.

---

## Installation / Instalação

```bash
dotnet add package ToonNet
```

---

## Usage / Uso

Below are examples showing how to **serialize** and **deserialize** objects using ToonNet.  
Abaixo estão exemplos mostrando como **serializar** e **desserializar** objetos usando o ToonNet.

---

### 1. Serialize using `ToonDocument` / Serializar com `ToonDocument`

```csharp
using ToonNet.Models;

var doc = new ToonDocument
{
    RootName = "users",
    Fields = new List<string> { "Id", "Name", "Role" },
    Rows = new List<string[]>
    {
        new[] { "1", "Alice", "admin" },
        new[] { "2", "Bob", "user" }
    }
};

string toon = doc.ToString();
Console.WriteLine(toon);
```

**Output / Saída:**
```
users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user
```

---

### 2. Deserialize from TOON text / Desserializar de texto TOON

```csharp
var toonText = @"users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user";

var document = ToonDocument.Parse(toonText);

Console.WriteLine(document.RootName); // "users"
Console.WriteLine(document.Fields[1]); // "Name"
Console.WriteLine(document.Rows[0][2]); // "admin"
```

**EN:** Converts back into a structured `ToonDocument`.  
**PT:** Converte novamente em um `ToonDocument` estruturado.

---

### 3. Serialize objects using `ToonSerializer` / Serializar objetos com `ToonSerializer`

```csharp
using ToonNet.Serialization;

var users = new List<User>
{
    new User { Id = 1, Name = "Alice", Role = "admin" },
    new User { Id = 2, Name = "Bob", Role = "user" }
};

string toon = ToonSerializer.Serialize(users, "users");

Console.WriteLine(toon);
```

**Output / Saída:**
```
users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user
```

---

### 4. Deserialize TOON text using `ToonDeserializer` / Desserializar texto TOON com `ToonDeserializer`

```csharp
using ToonNet.Serialization;

var toonText = @"users[2]{Id,Name,Role}:
  1,Alice,admin
  2,Bob,user";

var users = ToonDeserializer.Deserialize<User>(toonText);

Console.WriteLine(users[0].Name); // "Alice"
Console.WriteLine(users[1].Role); // "user"

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
}
```

**EN:** Converts TOON text directly into a list of typed objects.  
**PT:** Converte o texto TOON diretamente em uma lista de objetos tipados.

---

## Tests / Testes

**EN:** ToonNet uses **xUnit**, **FluentAssertions**, and **Bogus** for unit testing.  
**PT:** ToonNet utiliza **xUnit**, **FluentAssertions** e **Bogus** para testes unitários.

```bash
dotnet test
```

---

## Motivation / Motivação

**EN:**  
AI APIs often charge by token usage. TOON reduces textual noise while preserving semantic meaning, providing a token-efficient way to represent structured data.

**PT:**  
APIs de IA geralmente cobram por uso de tokens. TOON reduz o ruído textual preservando o significado semântico, oferecendo uma forma eficiente de representar dados estruturados.

---

## Contributing / Contribuindo

**EN:**  
Contributions are welcome! Fork the repository, create a new branch, commit your changes, and open a pull request.

**PT:**  
Contribuições são bem-vindas! Faça um fork do repositório, crie uma nova branch, envie suas alterações e abra um pull request.

---

## License / Licença

**MIT License** — see the [LICENSE](LICENSE) file for details.  
Licenciado sob **MIT** — consulte o arquivo [LICENSE](LICENSE) para mais detalhes.
