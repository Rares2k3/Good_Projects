package com.utcn.demo;

import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/api/ai")
@CrossOrigin(origins = "*")
public class ChatBotController {
    private final ChatBotService chatbotService;

    public ChatBotController(ChatBotService chatbotService) {
        this.chatbotService = chatbotService;
    }

    @PostMapping("/ask")
    public String askAI(@RequestBody String query){
        String systemPrompt = "Esti un asistent util pentru un magazin online de electronice." +
                "Raspunde scurt, in romana. Clientul intreaba: " + query;
        return chatbotService.getRecommandation(systemPrompt);
    }
}
