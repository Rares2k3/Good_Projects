package com.utcn.demo;

import org.springframework.web.bind.annotation.*;
import java.util.List;

@RestController
@RequestMapping("/products")
@CrossOrigin(origins = {"http://localhost:8080", "http://localhost:8081"})
public class ProductController {
    private final ProductService productService;

    public ProductController(ProductService productService) {
        this.productService = productService;
    }


    @GetMapping
    public List<Product> listProducts() {
        return productService.listProducts();
    }

    // ADAUGARE
    @PostMapping
    public Product addProduct(@RequestBody Product product) {
        return productService.addProduct(product);
    }

    // EDITARE (Noul buton)
    @PutMapping("/{id}")
    public Product updateProduct(@PathVariable Integer id, @RequestBody Product product) {
        return productService.updateProduct(id, product);
    }

    // STERGERE
    @DeleteMapping("/{id}")
    public String deleteProduct(@PathVariable Integer id) {
        productService.deleteProduct(id);
        return "Product deleted successfully!";
    }

    // ALTE ENDPOINT-URI
    @PutMapping("/{productId}/assignCategory/{categoryId}")
    public Product assignProductToCategory(@PathVariable Integer productId, @PathVariable Integer categoryId) {
        return productService.assignProductToCategory(productId, categoryId);
    }

    @GetMapping("/avg-price-per-category")
    public List<Object[]> getProductsPerCategoryWithAvgPrice() {
        return productService.getProductsPerCategoryWithAvgPrice();
    }

    @GetMapping("/top-rated")
    public List<Product> getTopRatedProducts() {
        return productService.getTopRatedProducts();
    }
}