<template>
  <v-container>
    <h1 class="text-h4 text-red-darken-4 mb-5 font-weight-bold">
      <v-icon start size="large">mdi-security</v-icon> Admin Dashboard
    </h1>

    <v-row>
      <v-col cols="12" md="4">
        <v-card elevation="3" class="pa-4 h-100">
          <v-card-title class="text-primary mb-2">
            <v-icon start>{{ isEditing ? 'mdi-pencil' : 'mdi-plus-box' }}</v-icon> 
            {{ isEditing ? 'Edit Product' : 'Add New Product' }}
          </v-card-title>
          
          <v-card-text>
            <v-form @submit.prevent="submitProduct" ref="form">
              
              <v-text-field 
                v-model="productForm.name" 
                label="Product Name" 
                variant="outlined" density="compact" class="mb-2"
              ></v-text-field>

              <v-text-field 
                v-model="productForm.price" 
                label="Price" type="number" 
                variant="outlined" density="compact" class="mb-2"
              ></v-text-field>

              <v-text-field 
                v-model="productForm.imageUrl" 
                label="Image URL" 
                placeholder="https://..."
                variant="outlined" density="compact" prepend-inner-icon="mdi-image" class="mb-2"
              ></v-text-field>
              
              <v-textarea 
                v-model="productForm.description" 
                label="Description" 
                variant="outlined" density="compact" rows="3" class="mb-2"
              ></v-textarea>

              <v-select
                v-model="selectedCategory"
                :items="categories"
                item-title="name"
                item-value="id"
                label="Category"
                variant="outlined" density="compact" return-object
              ></v-select>

              <v-btn type="submit" :color="isEditing ? 'warning' : 'success'" block class="mt-4 font-weight-bold">
                {{ isEditing ? 'Update Product' : 'Save Product' }}
              </v-btn>
              
              <v-btn v-if="isEditing" @click="cancelEdit" variant="text" color="grey" block class="mt-2">
                Cancel Edit
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="8">
        <v-card elevation="3" class="h-100">
          <v-card-title class="bg-grey-lighten-3">
            <v-icon start>mdi-format-list-bulleted</v-icon> Product Inventory
            <v-spacer></v-spacer>
            <v-btn size="small" icon="mdi-refresh" @click="loadProducts"></v-btn>
          </v-card-title>
          
          <v-table density="compact" fixed-header height="500px">
            <thead>
              <tr>
                <th>Img</th> <th>Name</th> <th>Category</th> <th>Price</th> <th class="text-center">Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="product in products" :key="product.id">
                <td>
                  <v-avatar size="32" rounded="0">
                    <v-img :src="product.imageUrl || 'https://via.placeholder.com/50'" cover></v-img>
                  </v-avatar>
                </td>
                <td class="font-weight-medium">{{ product.name }}</td>
                <td>
                  <v-chip size="x-small" color="blue" v-if="product.category">{{ product.category.name }}</v-chip>
                  <span v-else class="text-grey">-</span>
                </td>
                <td>{{ product.price }} RON</td>
                <td class="text-center">
                  <v-btn icon="mdi-pencil" size="small" color="warning" variant="text" class="mr-2" @click="startEdit(product)"></v-btn>
                  <v-btn icon="mdi-delete" size="small" color="error" variant="text" @click="deleteProduct(product.id)"></v-btn>
                </td>
              </tr>
            </tbody>
          </v-table>
        </v-card>
      </v-col>
    </v-row>

    <v-snackbar v-model="snackbar.show" :color="snackbar.color" location="top">
      {{ snackbar.message }}
      <template v-slot:actions><v-btn variant="text" @click="snackbar.show = false">Close</v-btn></template>
    </v-snackbar>
  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: "AdminPanel",
  data() {
    return {
      productForm: { name: '', price: '', description: '', imageUrl: '' },
      selectedCategory: null,
      
      // Stare pentru Editare
      isEditing: false,
      editingId: null,

      categories: [],
      products: [], 
      snackbar: { show: false, message: '', color: '' },
      
      
      backendUrl: 'http://localhost:8080' 
    };
  },
  methods: {
    async loadProducts() {
      try {
        const response = await axios.get(`${this.backendUrl}/products`);
        this.products = response.data;
      } catch (e) { console.error(e); }
    },
    async loadCategories() {
      try {
        const response = await axios.get(`${this.backendUrl}/categories/list`);
        this.categories = response.data;
      } catch (e) { console.error(e); }
    },

    // --- FUNCTIA UNIFICATA: SAVE sau UPDATE ---
    async submitProduct() {
      if (!this.productForm.name || !this.productForm.price || !this.selectedCategory) {
        this.showMsg("Please fill in Name, Price and Category!", "warning");
        return;
      }

      // Pregatim datele
      const payload = { ...this.productForm, category: this.selectedCategory };

      try {
        if (this.isEditing) {
          // MODUL UPDATE (PUT)
          await axios.put(`${this.backendUrl}/products/${this.editingId}`, payload);
          this.showMsg("Product updated successfully!", "success");
        } else {
          // MODUL ADD (POST)
          await axios.post(`${this.backendUrl}/products`, payload);
          this.showMsg("Product added successfully!", "success");
        }
        
        // Resetam formularul si reincarcam lista
        this.resetForm();
        this.loadProducts();

      } catch (error) {
        this.showMsg("Error saving product!", "error");
        console.error(error);
      }
    },

    // --- LOGICA DE EDITARE ---
    startEdit(product) {
      this.isEditing = true;
      this.editingId = product.id;
      
      // Copiem datele produsului in formular
      this.productForm = {
        name: product.name,
        price: product.price,
        description: product.description,
        imageUrl: product.imageUrl
      };
      // Selectam categoria corecta
      this.selectedCategory = product.category;
    },

    cancelEdit() {
      this.resetForm();
    },

    resetForm() {
      this.productForm = { name: '', price: '', description: '', imageUrl: '' };
      this.selectedCategory = null;
      this.isEditing = false;
      this.editingId = null;
    },

    async deleteProduct(id) {
      if(!confirm("Are you sure?")) return;
      try {
        await axios.delete(`${this.backendUrl}/products/${id}`);
        this.showMsg("Product deleted!", "info");
        this.loadProducts();
      } catch (error) { this.showMsg("Error deleting", "error"); }
    },
    showMsg(msg, color) {
      this.snackbar.message = msg;
      this.snackbar.color = color;
      this.snackbar.show = true;
    }
  },
  mounted() {
    // Verificare simplificata user
    const user = JSON.parse(localStorage.getItem('user'));
    // if (!user || user.role !== 'ADMIN') window.location.href = "/";
    
    this.loadCategories();
    this.loadProducts();
  }
};
</script>