
Imports System.ComponentModel.DataAnnotations
Imports BibliotecaAutomacaoFaturas
Imports MongoDB.Bson
Imports MongoDB.Bson.Serialization
Imports MongoDB.Driver

Public Class MongoDb

    Private db As IMongoDatabase

    Sub New(database As String)
        Dim client As New MongoClient("mongodb+srv://Jesse:VaThklsWs7i9j1SH@cluster0-fasvt.mongodb.net")

        'a anotação abaixo serve para mapear classes que agem de maneira diferente
        On Error Resume Next
        BsonClassMap.RegisterClassMap(Of CreditosMovelTim)()
        BsonClassMap.RegisterClassMap(Of TotalMovelTim)()
        BsonClassMap.RegisterClassMap(Of TotalMovelClaro)()
        BsonClassMap.RegisterClassMap(Of CreditosMovelClaro)()
        BsonClassMap.RegisterClassMap(Of EncargosMovelClaro)()
        BsonClassMap.RegisterClassMap(Of JurosEMulta)()





        'BsonClassMap.RegisterClassMap(Of PacoteNossoModoPlus)()
        On Error GoTo 0

        db = client.GetDatabase(database)
    End Sub

    Public Sub InserRecord(Of T)(table As String, record As T)
        Dim collection = db.GetCollection(Of T)(table)
        collection.InsertOne(record)
    End Sub

    Public Function LoadRecords(Of T)(table As String) As List(Of T)
        Dim collection = db.GetCollection(Of T)(table)
        Return collection.Find(New BsonDocument).ToList
    End Function

    Public Function LoadRecordById(Of T)(table As String, id As Guid)
        Dim collection = db.GetCollection(Of T)(table)
        Dim filter = Builders(Of T).Filter.Eq(Of Guid)("Id", id)
        Return collection.Find(filter).First
    End Function

    Public Sub UpsertRecor(Of T)(table As String, id As ObjectId, record As T)
        Dim collection = db.GetCollection(Of T)(table)
        Dim result = collection.ReplaceOne(New BsonDocument("_id", id),
                record, New UpdateOptions With {.IsUpsert = True})
    End Sub

    Public Sub DeleRecord(Of T)(table As String, id As Guid)
        Dim collection = db.GetCollection(Of T)(table)
        Dim filter = Builders(Of T).Filter.Eq(Of Guid)("Id", id)
        collection.DeleteOne(filter)

    End Sub

    Public Sub UpsertRecor(Of T)(table As String, CNPJ As String, record As T)
        Dim collection = db.GetCollection(Of T)(table)
        Dim result = collection.ReplaceOne(New BsonDocument("CNPJ", CNPJ),
                record, New UpdateOptions With {.IsUpsert = True})
    End Sub


    Public Sub DeleRecord(Of T)(table As String, CNPJ As String)
        Dim collection = db.GetCollection(Of T)(table)
        Dim filter = Builders(Of T).Filter.Eq(Of String)("CNPJ", CNPJ)
        collection.DeleteOne(filter)

    End Sub

    Public Sub UpsertRecord(Empresa As Empresa)
        validar(Empresa)

        Dim collection = db.GetCollection(Of Empresa)("Empresas")
        Dim result = collection.ReplaceOne(New BsonDocument("CNPJ", Empresa.CNPJ),
                Empresa, New UpdateOptions With {.IsUpsert = True})

    End Sub

    Public Sub UpsertRecord(Conta As Conta)
        validar(Conta)

        Dim collection = db.GetCollection(Of Conta)("Contas")
        Dim result = collection.ReplaceOne(New BsonDocument("NrDaConta", Conta.NrDaConta),
                Conta, New UpdateOptions With {.IsUpsert = True})

    End Sub

    Public Sub UpsertRecord(Gestor As Gestor)
        validar(Gestor)

        Dim collection = db.GetCollection(Of Gestor)("Gestores")
        Dim result = collection.ReplaceOne(New BsonDocument("CPF", Gestor.CPF),
                    Gestor, New UpdateOptions With {.IsUpsert = True})

    End Sub

    Public Sub DeleTarConta(Conta As Conta)
        Dim collection = db.GetCollection(Of Conta)("Contas")
        Dim filter = Builders(Of Conta).Filter.Eq(Of String)("NrDaConta", Conta.NrDaConta)
        collection.DeleteOne(filter)

    End Sub


    Friend Sub DeleTarGestor(gestor As Gestor)
        Dim collection = db.GetCollection(Of Conta)("Gestores")
        Dim filter = Builders(Of Conta).Filter.Eq(Of String)("CPF", gestor.CPF)
        collection.DeleteOne(filter)
    End Sub

    Public Function ChecarExistencia(empresa As Empresa) As Boolean

        Dim collection = db.GetCollection(Of Gestor)("Empresas")
        Dim result = collection.Count(New BsonDocument("CNPJ", empresa.CNPJ))

        If result > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function ChecarExistencia(conta As Conta) As Boolean

        Dim collection = db.GetCollection(Of Gestor)("Contas")
        Dim result = collection.Count(New BsonDocument("NrDaConta", conta.NrDaConta))

        result = collection.CountDocuments(New BsonDocument("NrDaConta", conta.NrDaConta))



        If result > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Function ChecarExistencia(Gestor As Gestor) As Boolean

        Dim collection = db.GetCollection(Of Gestor)("Gestores")
        Dim result = collection.Count(New BsonDocument("CPF", Gestor.CPF))

        If result > 0 Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub validar(objeto As Object)
        'Dim result As ICollection(Of ValidationResult)
        'result = New List(Of ValidationResult)
        Dim context = New ValidationContext(objeto)
        Validator.ValidateObject(objeto, context, True)

    End Sub

End Class
