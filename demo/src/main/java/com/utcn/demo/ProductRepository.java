package com.utcn.demo;

import org.springframework.stereotype.Repository;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import java.util.List;
@Repository
public interface ProductRepository extends JpaRepository<Product, Integer> {
    @Query("SELECT p.category.name, AVG(p.price) FROM Product p GROUP BY p.category.name")
    List<Object[]> getProductsPerCategoryWithAvgPrice();

    @Query("Select p FROM Product p JOIN p.reviews r GROUP BY p.id ORDER BY AVG(r.rating) DESC")
    List<Product> getTopRatedProducts();
}
