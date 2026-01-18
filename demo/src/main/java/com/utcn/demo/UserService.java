package com.utcn.demo;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

@Service
public class UserService {

    @Autowired
    private UserRepository userRepository;

    public User authenticate(String username, String password) {
        return userRepository.findByUsernameAndPassword(username, password).orElse(null);
    }

    public User registerUser(User newUser) {
        newUser.setRole("CUSTOMER");
        return userRepository.save(newUser);
    }
}