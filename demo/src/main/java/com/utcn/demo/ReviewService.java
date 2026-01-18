package com.utcn.demo;

import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class ReviewService {
    private final ReviewRepository reviewRepository;
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;
    public ReviewService(ReviewRepository reviewRepository, ProductRepository productRepository, CategoryRepository categoryRepository) {
        this.reviewRepository = reviewRepository;
        this.productRepository = productRepository;
        this.categoryRepository = categoryRepository;
    }

    public Review addReview(Integer productId,Review review) {
        Product product = productRepository.findById(productId).orElse(null);
        review.setProduct(product);
        return reviewRepository.save(review);
    }

    public List<Review> listReviews() {
        return reviewRepository.findAll();
    }
}
