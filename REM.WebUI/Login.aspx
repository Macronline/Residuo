<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" EnableEventValidation="false" %>

<!DOCTYPE html>
<html>
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Login</title>

  <!-- Bootstrap4 css -->
  <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous">
  <!-- Page css -->
  <link rel="stylesheet" href="css/login.css">
  <!-- Fonts -->
  <link href="https://fonts.googleapis.com/css2?family=Montserrat&display=swap" rel="stylesheet">

</head>
	<body >
        <form id="form1" runat="server">
		  <div class="container-fluid">
    <!-- row -->
    <div class="row">
      <!-- col-4 -->
      <div class="col-4"></div>
      
      <!-- col-4 -->
      <div class="col-4">
        <!-- margin-top -->
        <div class="top-margin"></div>

        <!-- section -->
        <section>
          <h1 class="title text-center">MyResiduo</h1>
          <div class="login text-center">
            <img  class="login-img" src="images/LogoMyResiduo.png" alt="dibujo camion de basura">
            <h2>Iniciar sesion</h2>
            <p>Ingrese sus datos de usuario para acceder al sistema</p>

            <!-- form -->
            <form action="">
              <div class="input-group mb-3">
                <div class="input-group-prepend">
                  <span class="input-group-text" id="basic-addon1">Usuario</span>
                </div>
                <%--<input type="text" class="form-control" placeholder="Ingrese su usuario">--%>
                  <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Ingrese su usuario"></asp:TextBox>
              </div>
              <div class="input-group mb-3">
                <div class="input-group-prepend">
                  <span class="input-group-text" id="basic-addon1">Contraseña</span>
                </div>
                <%--<input type="password" class="form-control" placeholder="Ingrese su contraseña">--%>
                  <asp:TextBox ID="txtPass" runat="server" class="form-control" placeholder="Ingrese su contraseña" TextMode="Password"></asp:TextBox>
              </div>
              <%--<button type="submit" class="btn btn-success">Ingresar</button>--%>
                <asp:Button ID="btnLogin" runat="server" Text="Ingresar" CssClass="btn btn-success" OnClick="btnLogin_Click" />
            </form>
            <!-- form -->

            <p class="forgot-password">Olvido su contraseña?</p>

          </div>
        </section>
        <!-- section -->
      </div>
      <!-- col-4 -->

      <!-- col-4 -->
      <div class="col-4"></div>
    </div>

    <!-- row -->
  </div>
  <!-- container -->

  <!-- Bootstrap4 js -->
  <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
  <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
  <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
  <!-- Bootstrap4 js -->
            </form>
</body>
</html>
