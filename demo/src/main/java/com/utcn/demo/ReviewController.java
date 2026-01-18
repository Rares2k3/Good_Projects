package com.utcn.demo;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;

import java.util.List;

public class ReviewController {
    private final ReviewService reviewService;

    public ReviewController(ReviewService reviewService) {
        this.reviewService = reviewService;
    }

    @PostMapping("/product{productId}")
    public Review addReview(@PathVariable Integer productId, @RequestBody Review review){
        return reviewService.addReview(productId, review);
    }

    @GetMapping
    public List<Review> getAllReviews() {
        return reviewService.listReviews();
    }
}
