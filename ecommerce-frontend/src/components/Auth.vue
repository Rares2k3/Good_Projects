<template>
  <v-container>
    <h1 class="text-center my-5 text-h4 font-weight-bold text-primary">Welcome!</h1>

    <v-row justify="center">
      <v-col cols="12" md="5">
        <v-card elevation="6" class="pa-4 rounded-lg">
          <v-card-title class="text-h5 text-center mb-4">🔐 Login</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="handleLogin">
              <v-text-field 
                v-model="loginData.username" 
                label="Username" 
                prepend-inner-icon="mdi-account" 
                variant="outlined"
                density="comfortable"
              ></v-text-field>

              <v-text-field 
                v-model="loginData.password" 
                label="Password" 
                type="password" 
                prepend-inner-icon="mdi-lock" 
                variant="outlined"
                density="comfortable"
                class="mt-2"
              ></v-text-field>
              
              <v-btn type="submit" block color="primary" size="large" class="mt-6 font-weight-bold">
                Sign In
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="1" class="d-flex align-center justify-center">
        <v-divider vertical class="hidden-sm-and-down"></v-divider>
        <span class="hidden-md-and-up my-4 text-grey">OR</span>
      </v-col>

      <v-col cols="12" md="5">
        <v-card elevation="6" class="pa-4 rounded-lg">
          <v-card-title class="text-h5 text-center mb-4">📝 Sign Up</v-card-title>
          <v-card-text>
            <v-form @submit.prevent="handleSignup">
              <v-text-field v-model="signupData.email" label="Email" prepend-inner-icon="mdi-email" variant="outlined" density="compact"></v-text-field>
              <v-text-field v-model="signupData.username" label="Username" prepend-inner-icon="mdi-account-plus" variant="outlined" density="compact"></v-text-field>
              <v-text-field v-model="signupData.password" label="Password" type="password" prepend-inner-icon="mdi-lock-plus" variant="outlined" density="compact"></v-text-field>
              <v-text-field v-model="signupData.address" label="Shipping Address" prepend-inner-icon="mdi-home" variant="outlined" density="compact"></v-text-field>
              <v-text-field v-model="signupData.phoneNumber" label="Phone Number" prepend-inner-icon="mdi-phone" variant="outlined" density="compact"></v-text-field>

              <v-btn type="submit" block color="secondary" size="large" class="mt-4 font-weight-bold">
                Create New Account
              </v-btn>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-snackbar v-model="snackbar.show" :color="snackbar.color" location="top">
      {{ snackbar.message }}
      <template v-slot:actions>
        <v-btn variant="text" @click="snackbar.show = false">Close</v-btn>
      </template>
    </v-snackbar>
  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: "AuthComponent",
  data() {
    return {
      // Data for Login form
      loginData: { username: '', password: '' },
      // Data for Sign Up form
      signupData: { email: '', username: '', password: '', address: '', phoneNumber: '' },
      // Notification config
      snackbar: { show: false, message: '', color: '' }
    };
  },
  methods: {
    // Login Function
    async handleLogin() {
      try {
        const response = await axios.post('http://localhost:8080/users/login', this.loginData);
        
        if (response.data) {
          // 1. Save user to browser
          localStorage.setItem('user', JSON.stringify(response.data));
          
          this.showMsg("Login successful! Loading...", "success");
          
          // 2. Wait slightly, then refresh
          setTimeout(() => {
            window.location.reload();
          }, 1000);
        } else {
          this.showMsg("Incorrect username or password!", "error");
        }
      } catch (error) {
        console.error(error);
        this.showMsg("Server connection error!", "error");
      }
    },

    // Sign Up Function
    async handleSignup() {
      // Simple validation
      if (!this.signupData.username || !this.signupData.password || !this.signupData.email) {
        this.showMsg("Please fill in all required fields!", "warning");
        return;
      }

      try {
        await axios.post('http://localhost:8080/users/signup', this.signupData);
        this.showMsg("Account created successfully! You may now login.", "success");
        // Reset form
        this.signupData = { email: '', username: '', password: '', address: '', phoneNumber: '' };
      } catch (error) {
        console.error(error);
        this.showMsg("Error creating account (user might already exist).", "error");
      }
    },

    // Helper for notifications
    showMsg(msg, color) {
      this.snackbar.message = msg;
      this.snackbar.color = color;
      this.snackbar.show = true;
    }
  }
};
</script>