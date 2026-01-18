<template>
  <v-app>
    <v-app-bar color="deep-purple-darken-2" elevation="3">
      <v-app-bar-title class="font-weight-bold">
        <v-icon icon="mdi-shopping" class="mr-2"></v-icon>
        E-Shop
      </v-app-bar-title>

      <template v-slot:extension>
        <v-tabs v-model="activeTab" align-tabs="center" color="white">
          <v-tab value="shop"><v-icon start>mdi-store</v-icon>Products</v-tab>
          <v-tab value="admin" v-if="isAdmin"><v-icon start>mdi-cog</v-icon>Admin Panel</v-tab>
          <v-tab value="contact"><v-icon start>mdi-email</v-icon>Contact</v-tab>
          <v-tab value="auth" v-if="!currentUser"><v-icon start>mdi-account-key</v-icon>Login</v-tab>
        </v-tabs>
      </template>

      <v-spacer></v-spacer>

      <div v-if="currentUser" class="d-flex align-center mr-4">
        <v-chip color="white" variant="outlined" class="mr-2">
          <v-icon start>mdi-account</v-icon>
          {{ currentUser.username }} 
          <span v-if="isAdmin" class="font-weight-bold ms-1">(ADMIN)</span>
        </v-chip>
        <v-btn icon="mdi-logout" size="small" variant="text" @click="logout" title="Logout"></v-btn>
      </div>

      <v-btn icon class="mr-2" @click="handleCartClick">
        <v-badge :content="cartCount" color="error" v-if="cartCount > 0">
          <v-icon>mdi-cart</v-icon>
        </v-badge>
        <v-icon v-else>mdi-cart-outline</v-icon>
      </v-btn>
    </v-app-bar>

    <v-main class="bg-grey-lighten-4">
      <v-window v-model="activeTab">
        
        <v-window-item value="shop">
          <ProductList ref="productList" />
        </v-window-item>

        <v-window-item value="admin" v-if="isAdmin">
          <AdminPanel />
        </v-window-item>

        <v-window-item value="contact">
          <v-container class="text-center mt-10"><h1>Contact Us</h1><p>support@eshop.com</p></v-container>
        </v-window-item>

        <v-window-item value="auth">
          <Auth />
        </v-window-item>

        <v-window-item value="cart">
           <ShoppingCart ref="cartComponent" />
        </v-window-item>

      </v-window>
    </v-main>
  </v-app>
</template>

<script>
import ProductList from './components/ProductList.vue';
import AdminPanel from './components/AdminPanel.vue';
import Auth from './components/Auth.vue';

// --- MODIFICARE IMPORTANTA AICI ---
// Importam fisierul nou (ShoppingCart.vue), nu pe cel vechi (Cart.vue)
import ShoppingCart from './components/ShoppingCart.vue'; 

export default {
  name: 'App',
  components: {
    ProductList,
    AdminPanel,
    Auth,
    ShoppingCart // Declaram componenta noua
  },
  data() {
    return {
      activeTab: 'shop',
      currentUser: null,
      cartCount: 0 
    };
  },
  computed: {
    isAdmin() {
      return this.currentUser && this.currentUser.role === 'ADMIN';
    }
  },
  watch: {
    activeTab(newTab) {
      if (newTab === 'shop') {
        this.$nextTick(() => { if (this.$refs.productList) this.$refs.productList.fetchProducts(); });
      }
      if (newTab === 'cart') {
        // Actualizam referinta catre componenta noua
        this.$nextTick(() => { if (this.$refs.cartComponent) this.$refs.cartComponent.loadCart(); });
      }
    }
  },
  methods: {
    handleCartClick() {
      if (this.currentUser) {
        this.activeTab = 'cart';
      } else {
        alert("Please login to view your cart!");
        this.activeTab = 'auth';
      }
    },
    logout() {
      localStorage.removeItem('user');
      this.currentUser = null;
      window.location.reload();
    },
    updateCartCount() {
      const cart = JSON.parse(localStorage.getItem('cart')) || [];
      this.cartCount = cart.reduce((acc, item) => acc + item.quantity, 0);
    }
  },
  mounted() {
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
      try {
        this.currentUser = JSON.parse(savedUser);
        if (this.activeTab === 'admin' && !this.isAdmin) this.activeTab = 'shop';
      } catch (e) { localStorage.removeItem('user'); }
    }

    this.updateCartCount();
    window.addEventListener('cart-updated', this.updateCartCount);
  },
  beforeUnmount() {
    window.removeEventListener('cart-updated', this.updateCartCount);
  }
};
</script>