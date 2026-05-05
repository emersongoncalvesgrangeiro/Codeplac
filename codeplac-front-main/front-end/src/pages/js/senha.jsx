import React, { useState } from "react";
import "../css/senha.css";
import Header from "../../Components/jsx/header";
import Footer from "../../Components/jsx/footer";
import Circle from "../../Components/jsx/circle";
import sapoImg from "../../assets/img/sapobone.png";

function Senha() {
  const urlfinal = `${process.env.REACT_APP_URL}/auth/forgot-password`.replace(
    /([^:]\/)\/+/g,
    "$1",
  );

  const [formData, setFormData] = useState({
    cpf: "",
  });
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };
  const handleSubmit = async (e) => {
    e.preventDefault();
    const camposVazios = Object.values(formData).some(
      (valor) => valor.trim() === "",
    );
    if (camposVazios) {
      alert("Por favor, preencha todos os campos!");
      return;
    }

    try {
      const response = await fetch(urlfinal, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(formData),
      });
      if (!response.ok) {
        const errorText = await response.text();
        console.error("Erro do Servidor:", errorText);
        try {
          const errorData = JSON.parse(errorText);
          alert(errorData.message || "Erro ao processar solicitação.");
        } catch {
          alert("Erro ao processar solicitação.");
        }
        return;
      }
      alert("Link de redefinição enviado para o seu e-mail!");
      window.location.href = "/login";
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div className="PageWrapper">
      <div className="App login-container">
        <Header />

        {/* Efeitos de Fundo Neon */}
        <div className=".recovery-circle">
          <Circle size={500} variant="cyan" className="recovery-circle-left" />
          <Circle size={400} variant="cyan" className="recovery-circle-right" />
        </div>

        <main className="login-main">
          <section className="login-card recovery-card">
            <div className="login-content">
              {/* Lado Esquerdo: Mascote */}
              <div className="login-mascot-container">
                <img
                  src={sapoImg}
                  alt="Mascote CodeplaC"
                  className="mascot-img"
                />
              </div>

              {/* Lado Direito: Formulário de Recuperação */}
              <div className="login-form-container">
                <h1 className="login-title uppercase">RECUPERE SUA CONTA</h1>
                <div className="title-underline"></div>

                <p className="recovery-instruction">
                  INFORME SEU CPF PARA RECEBER UM LINK <br />
                  DE REDEFINIÇÃO POR E-MAIL.
                </p>

                <form className="login-form" onSubmit={handleSubmit}>
                  <div className="login-input-group">
                    <label>CPF</label>
                    <input
                      type="text"
                      placeholder="000.000.000-00"
                      name="cpf"
                      value={formData.cpf}
                      onChange={handleChange}
                    />
                  </div>

                  <button type="submit" className="btn-login btn-send">
                    ENVIAR LINK
                  </button>
                </form>

                <div className="create-account">
                  <a href="/login" className="back-to-login">
                    VOLTAR PARA O LOGIN
                  </a>
                </div>
              </div>
            </div>
          </section>
        </main>

        <Footer />
      </div>
    </div>
  );
}

export default Senha;
