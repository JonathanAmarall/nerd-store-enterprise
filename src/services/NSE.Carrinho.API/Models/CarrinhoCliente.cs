﻿using System;
using System.Collections.Generic;

namespace NSE.Carrinho.API.Models
{
    public class CarrinhoCliente
    {
        public CarrinhoCliente()
        {

        }

        public CarrinhoCliente(Guid clienteId)
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
        }

        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public decimal ValorTotal { get; set; }
        public List<CarrinhoItem> Itens { get; set; } = new List<CarrinhoItem>();
    }
}