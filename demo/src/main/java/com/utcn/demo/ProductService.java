package com.utcn.demo;

import org.springframework.stereotype.Service;
import java.util.List;

@Service
public class ProductService {
    private final ProductRepository productRepository;
    private final CategoryRepository categoryRepository;

    public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository) {
        this.productRepository = productRepository;
        this.categoryRepository = categoryRepository;
    }

    // --- METODA 1: ADAUGARE (CREATE) ---
    public Product addProduct(Product product) {
        return productRepository.save(product);
    }

    // --- METODA 2: LISTARE (READ) - Aici e posibil sa fi fost problema ---
    public List<Product> listProducts() {
        return productRepository.findAll();
    }

    // --- METODA 3: ACTUALIZARE (UPDATE) ---
    public Product updateProduct(Integer id, Product productDetails) {
        Product product = productRepository.findById(id).orElse(null);
        if (product != null) {
            product.setName(productDetails.getName());
            product.setPrice(productDetails.getPrice());
            product.setDescription(productDetails.getDescription());
            product.setImageUrl(productDetails.getImageUrl());

            if (productDetails.getCategory() != null) {
                product.setCategory(productDetails.getCategory());
            }
            return productRepository.save(product);
        }
        return null;
    }

    // --- METODA 4: STERGERE (DELETE) ---
    public void deleteProduct(Integer id) {
        productRepository.deleteById(id);
    }

    // --- METODE AUXILIARE ---
    public Product assignProductToCategory(Integer productId, Integer categoryId) {
        Product product = productRepository.findById(productId).orElse(null);
        Category category = categoryRepository.findById(categoryId).orElse(null);
        if (product != null && category != null) {
            product.setCategory(category);
            return productRepository.save(product);
        }
        return null;
    }

    public List<Object[]> getProductsPerCategoryWithAvgPrice() {
        return productRepository.getProductsPerCategoryWithAvgPrice();
    }

    public List<Product> getTopRatedProducts(){
        return productRepository.getTopRatedProducts();
    }
}