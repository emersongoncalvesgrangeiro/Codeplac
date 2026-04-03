package codeplac.codeplac.Security;

import java.util.Arrays;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.HttpMethod;
import org.springframework.security.authentication.AuthenticationManager;
import org.springframework.security.config.annotation.authentication.configuration.AuthenticationConfiguration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;
import org.springframework.web.cors.CorsConfiguration;
import org.springframework.web.cors.CorsConfigurationSource;
import org.springframework.web.cors.UrlBasedCorsConfigurationSource;

@Configuration
@EnableWebSecurity
public class SecurityConfig {

    private static final Logger logger = LoggerFactory.getLogger(SecurityConfig.class);

    @Autowired
    private SecurityFilter securityFilter;

    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
        logger.info("Configurando a cadeia de filtros de segurança...");

        http
                // 2. CONFIGURA O CORS ANTES DE TUDO
                .cors(cors -> cors.configurationSource(corsConfigurationSource()))
                .csrf(csrf -> csrf.disable())
                // 3. ESTADO STATELESS
                .authorizeHttpRequests(authorize -> authorize
                .requestMatchers("/error").permitAll() // Permite acesso ao endpoint de erro
                .requestMatchers(HttpMethod.POST, "/users/register").permitAll()
                .requestMatchers(HttpMethod.OPTIONS, "/**").permitAll()
                // 1. LIBERAÇÕES PÚBLICAS (SWAGGER E REGISTRO NO TOPO)
                .requestMatchers("/v3/api-docs/**", "/swagger-ui/**", "/swagger-ui.html").permitAll()
                // 2. OUTRAS ROTAS PÚBLICAS
                .requestMatchers(HttpMethod.HEAD, "/teste").permitAll()
                .requestMatchers(HttpMethod.GET, "/teste").permitAll()
                .requestMatchers(HttpMethod.POST, "/auth/login").permitAll()
                .requestMatchers(HttpMethod.POST, "/auth/forgot-password").permitAll()
                .requestMatchers(HttpMethod.POST, "/auth/reset-password").permitAll()
                .requestMatchers(HttpMethod.POST, "/juizcodigo").permitAll()
                .requestMatchers(HttpMethod.POST, "/equipes/inscricao").permitAll()
                .requestMatchers(HttpMethod.GET, "/event/list").permitAll()
                .requestMatchers(HttpMethod.GET, "/event/{id}").permitAll()
                .requestMatchers(HttpMethod.POST, "/juntese").permitAll()
                .requestMatchers(HttpMethod.POST, "/juntese").permitAll()
                .requestMatchers(HttpMethod.GET, "/ranking/{id}").permitAll()
                //.requestMatchers(HttpMethod.PUT, "/group/modify/{id}").permitAll()
                // 3. ROTAS PROTEGIDAS POR ROLE
                .requestMatchers(HttpMethod.POST, "/event/create").hasRole("ADMIN")
                .requestMatchers(HttpMethod.POST, "/registration/create").hasAnyRole("ADMIN", "PARTICIPANT")
                .requestMatchers(HttpMethod.GET, "/users/list").hasRole("ADMIN")
                .requestMatchers(HttpMethod.GET, "/users/{cpf}").hasAnyRole("ADMIN", "PARTICIPANT")
                .requestMatchers(HttpMethod.PUT, "/users/modify/{cpf}").hasAnyRole("ADMIN", "PARTICIPANT")
                .requestMatchers(HttpMethod.DELETE, "/users/destroy/{cpf}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.PUT, "/event/modify/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.DELETE, "/event/destroy/{id}").hasRole("ADMIN")
                //.requestMatchers(HttpMethod.GET, "/group/list").hasRole("ADMIN")
                //.requestMatchers(HttpMethod.GET, "/group/{id}").hasRole("ADMIN")
                //.requestMatchers(HttpMethod.DELETE, "/group/destroy/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.POST, "/ranking/create").hasRole("ADMIN")
                .requestMatchers(HttpMethod.GET, "/ranking/list").hasRole("ADMIN")
                .requestMatchers(HttpMethod.PUT, "/ranking/modify/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.DELETE, "/ranking/destroy/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.GET, "/registration/list").hasRole("ADMIN")
                .requestMatchers(HttpMethod.GET, "/registration/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.PUT, "/registration/modify/{id}").hasRole("ADMIN")
                .requestMatchers(HttpMethod.DELETE, "/registration/destroy/{id}").hasRole("ADMIN")
                .requestMatchers("/admin/**").hasRole("ADMIN")
                // 4. QUALQUER OUTRA REQUISIÇÃO EXIGE AUTENTICAÇÃO
                .anyRequest().authenticated()
                ).addFilterBefore(securityFilter, UsernamePasswordAuthenticationFilter.class);
        //.addFilterBefore(securityFilter, UsernamePasswordAuthenticationFilter.class);

        return http.build();
    }

    @Bean
    public CorsConfigurationSource corsConfigurationSource() {
        CorsConfiguration config = new CorsConfiguration();
        config.setAllowedOrigins(Arrays.asList(
                "http://localhost:3000",
                "http://127.0.0.1:3000",
                "https://kkz8xgr7-3000.brs.devtunnels.ms",
                "http://localhost:8080",
                "http://127.0.0.1:8080",
                "http://127.0.0.1:5500",
                "https://www.codeplac.com.br",
                "https://codeplac.com.br",
                "https://codeplac-vh95.onrender.com"
        ));
        config.setAllowedMethods(Arrays.asList("GET", "POST", "PUT", "DELETE", "OPTIONS"));
        config.setAllowedHeaders(Arrays.asList("Authorization", "Content-Type", "Accept"));
        config.setAllowCredentials(true);
        config.setMaxAge(3600L);

        UrlBasedCorsConfigurationSource source = new UrlBasedCorsConfigurationSource();
        source.registerCorsConfiguration("/**", config);
        return source;
    }

    @Bean
    public PasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }

    @Bean
    public AuthenticationManager authenticationManager(AuthenticationConfiguration authenticationConfiguration) throws Exception {
        return authenticationConfiguration.getAuthenticationManager();
    }
}
