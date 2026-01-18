package com.utcn.demo;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;
import java.util.HashMap;
import java.util.Map;
@Service
public class ChatBotService {
    private final String ollamaModel = "gemma3:4b";
    private final String ollamaUrl = "http://localhost:11434/api/chat";

    public String getRecommandation(String promptUser){
        RestTemplate restTemplate = new RestTemplate();
        Map<String, Object> requestBody = new HashMap<>();
        requestBody.put("model", ollamaModel);
        requestBody.put("prompt", promptUser);
        requestBody.put("stream", false);
        try {
            Map response = restTemplate.postForObject(ollamaUrl, requestBody, Map.class);

            if (response != null && response.containsKey("response")){
                return response.get("response").toString();
            } else {
                return "AI-ul nu a generat nicun răspuns.";
            }

        }catch (Exception e){
            e.printStackTrace();
            return "Eroare la comunicarea cu AI-ul: " + e.getMessage();
        }
    }
}
