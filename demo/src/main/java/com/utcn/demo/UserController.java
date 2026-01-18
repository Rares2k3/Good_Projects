package com.utcn.demo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

@RestController
@RequestMapping("/users")
@CrossOrigin(originPatterns = "*")
public class UserController {

    @Autowired
    private UserService userService;

    @PostMapping("/login")
    public User login(@RequestBody User loginData) {
        return userService.authenticate(loginData.getUsername(), loginData.getPassword());
    }

    @PostMapping("/signup")
    public User signup(@RequestBody User newUser) {
        return userService.registerUser(newUser);
    }
}