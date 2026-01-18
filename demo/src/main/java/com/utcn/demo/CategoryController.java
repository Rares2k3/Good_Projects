package com.utcn.demo;

import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/categories")
public class CategoryController {
    private final CategoryService categoryService;

    public CategoryController(CategoryService categoryService) {
        this.categoryService = categoryService;
    }

    @PostMapping("/add")
    public Category addCategory(@RequestBody Category category) {
        return categoryService.addCategory(category);
    }

    @GetMapping("/list")
    public List<Category> listCategories() {
        return categoryService.listCategories();
    }
}
