<template>
  <v-container>
    <v-row class="mb-4" align="center">
      <v-col cols="12" md="4"><h1 class="text-h4 font-weight-bold text-primary">Product Catalog</h1></v-col>
      <v-col cols="12" md="4">
        <v-text-field v-model="searchQuery" label="Search..." prepend-inner-icon="mdi-magnify" variant="outlined" density="compact" hide-details @input="applyFilters"></v-text-field>
      </v-col>
      <v-col cols="12" md="4">
        <v-select v-model="selectedCategory" :items="categories" label="Category" variant="outlined" density="compact" hide-details @update:modelValue="applyFilters"></v-select>
      </v-col>
    </v-row>

    <v-divider class="mb-5"></v-divider>

    <v-row v-if="filteredProducts.length > 0">
      <v-col v-for="product in filteredProducts" :key="product.id" cols="12" sm="6" md="4" lg="3">
        <v-card elevation="4" class="h-100 d-flex flex-column hover-card">
          
          <v-img
            :src="product.imageUrl ? product.imageUrl : 'https://via.placeholder.com/300x200?text=No+Image'"
            height="200px"
            cover
            class="bg-grey-lighten-2"
          ></v-img>

          <v-card-item>
            <v-card-title class="text-h6 font-weight-bold">{{ product.name }}</v-card-title>
            <v-card-subtitle class="mb-2">
              <v-chip size="x-small" color="primary" variant="flat" v-if="product.category">{{ product.category.name }}</v-chip>
            </v-card-subtitle>
          </v-card-item>

          <v-card-text class="flex-grow-1">
            <div class="text-h5 text-success font-weight-bold mb-2">{{ product.price }} RON</div>
            
            <p class="text-body-2 text-grey-darken-1">
              {{ product.description || 'No description available for this product.' }}
            </p>
          </v-card-text>

          <v-divider></v-divider>

          <v-card-actions>
            <v-btn variant="elevated" color="deep-purple-accent-3" block class="text-white font-weight-bold" @click="addToCart(product)">
              <v-icon start>mdi-cart-plus</v-icon> ADD TO CART
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>

    <v-alert v-else type="info" variant="tonal" class="mt-5">No products found.</v-alert>

    <v-snackbar v-model="snackbar.show" color="success" location="bottom right" timeout="3000">
      <v-icon start>mdi-check-circle</v-icon> {{ snackbar.message }}
    </v-snackbar>
  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: 'ProductList',
  data() {
    return {
      allProducts: [], filteredProducts: [], categories: ["All Categories"],
      selectedCategory: "All Categories", searchQuery: "", snackbar: { show: false, message: "" }
    };
  },
  methods: {
    async fetchProducts() {
      try {
        const response = await axios.get('http://localhost:8080/products');
        this.allProducts = response.data;
        this.applyFilters();
      } catch (e) { console.error(e); }
    },
    async fetchCategories() {
      try {
        const res = await axios.get('http://localhost:8080/categories/list');
        this.categories = ["All Categories", ...res.data.map(c => c.name)];
      } catch (e) { console.error(e); }
    },
    applyFilters() {
      this.filteredProducts = this.allProducts.filter(p => {
        const catMatch = this.selectedCategory === "All Categories" || (p.category && p.category.name === this.selectedCategory);
        const nameMatch = p.name.toLowerCase().includes(this.searchQuery.toLowerCase());
        return catMatch && nameMatch;
      });
    },
    addToCart(product) {
      let cart = JSON.parse(localStorage.getItem('cart')) || [];
      let existing = cart.find(i => i.id === product.id);
      if (existing) existing.quantity++;
      else cart.push({ ...product, quantity: 1 });
      
      localStorage.setItem('cart', JSON.stringify(cart));
      window.dispatchEvent(new Event('cart-updated'));
      this.snackbar.message = `Added "${product.name}" to cart!`;
      this.snackbar.show = true;
    }
  },
  mounted() {
    this.fetchProducts();
    this.fetchCategories();
  }
};
</script>