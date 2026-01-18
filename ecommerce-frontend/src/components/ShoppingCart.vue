<template>
  <v-container>
    <h1 class="text-h4 font-weight-bold text-primary mb-5">🛒 Your Shopping Cart</h1>

    <v-row>
      <v-col cols="12" md="8">
        <v-card elevation="3" class="pa-4" v-if="cartItems.length > 0">
          <v-list lines="two">
            <template v-for="(item, index) in cartItems" :key="item.id">
              <v-list-item>
                <template v-slot:prepend>
                  <v-avatar size="80" rounded="0">
                    <v-img :src="getProductImage(item)" cover></v-img>
                  </v-avatar>
                </template>

                <v-list-item-title class="text-h6 font-weight-bold ml-4">
                  {{ item.name }}
                </v-list-item-title>
                
                <v-list-item-subtitle class="ml-4 mt-1">
                  Unit Price: <span class="text-success font-weight-bold">{{ item.price }} RON</span>
                </v-list-item-subtitle>

                <template v-slot:append>
                  <div class="d-flex align-center">
                    <v-btn icon="mdi-minus" size="x-small" variant="text" @click="decreaseQty(index)"></v-btn>
                    <span class="mx-2 text-h6">{{ item.quantity }}</span>
                    <v-btn icon="mdi-plus" size="x-small" variant="text" @click="increaseQty(index)"></v-btn>
                    
                    <v-btn icon="mdi-delete" color="error" variant="text" class="ml-4" @click="removeFromCart(index)"></v-btn>
                  </div>
                </template>
              </v-list-item>
              <v-divider v-if="index < cartItems.length - 1"></v-divider>
            </template>
          </v-list>
        </v-card>

        <v-alert v-else type="info" variant="tonal" icon="mdi-cart-off">
          Your cart is currently empty. Go to Products to add some items!
        </v-alert>
      </v-col>

      <v-col cols="12" md="4" v-if="cartItems.length > 0">
        <v-card elevation="3" class="pa-4">
          <v-card-title>Order Summary</v-card-title>
          <v-divider class="my-3"></v-divider>
          
          <div class="d-flex justify-space-between mb-2">
            <span>Subtotal:</span>
            <span>{{ totalPrice }} RON</span>
          </div>
          <div class="d-flex justify-space-between mb-4">
            <span>Shipping:</span>
            <span class="text-success">FREE</span>
          </div>
          
          <v-divider></v-divider>
          
          <div class="d-flex justify-space-between my-4 text-h5 font-weight-bold">
            <span>Total:</span>
            <span class="text-primary">{{ totalPrice }} RON</span>
          </div>

          <v-btn block color="success" size="large" @click="checkout">
            Proceed to Checkout
          </v-btn>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "ShoppingCart",
  data() {
    return {
      cartItems: []
    };
  },
  computed: {
    totalPrice() {
      return this.cartItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    }
  },
  methods: {
    // --- SMART IMAGE FUNCTION (Duplicate) ---
    getProductImage(product) {
      const name = product.name.toLowerCase();
      if (name.includes('macbook') || name.includes('laptop')) return 'https://images.unsplash.com/photo-1517336714731-489689fd1ca4?w=500';
      if (name.includes('iphone') || name.includes('samsung') || name.includes('galaxy')) return 'https://images.unsplash.com/photo-1510557880182-3d4d3cba35a5?w=500';
      if (name.includes('playstation') || name.includes('ps5') || name.includes('gaming')) return 'https://images.unsplash.com/photo-1606144042614-b2417e99c4e3?w=500';
      if (name.includes('mouse') || name.includes('keyboard')) return 'https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?w=500';
      if (name.includes('casti') || name.includes('headphone')) return 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500';
      return `https://picsum.photos/seed/${product.id}/300/200`;
    },

    loadCart() {
      const storedCart = localStorage.getItem('cart');
      this.cartItems = storedCart ? JSON.parse(storedCart) : [];
    },
    updateStorage() {
      localStorage.setItem('cart', JSON.stringify(this.cartItems));
      window.dispatchEvent(new Event('cart-updated'));
    },
    increaseQty(index) {
      this.cartItems[index].quantity++;
      this.updateStorage();
    },
    decreaseQty(index) {
      if (this.cartItems[index].quantity > 1) {
        this.cartItems[index].quantity--;
        this.updateStorage();
      }
    },
    removeFromCart(index) {
      this.cartItems.splice(index, 1);
      this.updateStorage();
    },
    checkout() {
      alert(`Order placed successfully! Total: ${this.totalPrice} RON`);
      this.cartItems = [];
      this.updateStorage();
    }
  },
  mounted() {
    this.loadCart();
  }
};
</script>