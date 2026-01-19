package com.utcn.demo;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/ai")
@CrossOrigin(origins = "http://localhost:8080", allowCredentials = "true")
public class ChatBotController {
    private final ChatBotService chatbotService;

    public ChatBotController(ChatBotService chatbotService) {
        this.chatbotService = chatbotService;
    }

    @PostMapping("/ask")
    public String askAI(@RequestBody String query){
        String systemPrompt = "You're a sales assistant expert in an IT shop." +
                "Your goal is to provide personalized recommendations for laptops and phones. " +
                "Be brief, polite, and help the customer choose. " +
                "The customer's question is: " + query;
        return chatbotService.getRecommandation(systemPrompt);
    }
}
