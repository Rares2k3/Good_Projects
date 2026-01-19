package com.utcn.demo;

import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import org.springframework.http.client.SimpleClientHttpRequestFactory;
import java.util.HashMap;
import java.util.Map;

@Service
public class ChatBotService {

    // 1. FOLOSIM UN MODEL REAL SI RAPID
    // Daca vrei altul, asigura-te ca apare la comanda "ollama list"
    private final String ollamaModel = "gemma3:4b";

    private final String ollamaUrl = "http://localhost:11434/api/generate";

    public String getRecommandation(String promptUser){
        // 2. CONFIGURARE TIMEOUT (RABDARE)
        SimpleClientHttpRequestFactory factory = new SimpleClientHttpRequestFactory();
        factory.setConnectTimeout(5000);  // 5 secunde sa se conecteze la Ollama
        factory.setReadTimeout(240000);   // 4 MINUTE sa astepte generarea raspunsului

        RestTemplate restTemplate = new RestTemplate(factory);

        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put("model", ollamaModel);
        requestBody.put("prompt", promptUser);
        requestBody.put("stream", false);

        try {
            // Trimitem cererea
            Map response = restTemplate.postForObject(ollamaUrl, requestBody, Map.class);

            if (response != null && response.containsKey("response")){
                return response.get("response").toString();
            } else {
                return "AI response was empty.";
            }

        } catch (Exception e){
            e.printStackTrace();
            // Mesaj de eroare in Engleza
            return "Server Error: Could not reach AI service. Reason: " + e.getMessage();
        }
    }
}